using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Guilded.Tests.Areas.Admin.RolesController
{
    public class WhenEditOrCreatePostIsCalled : RolesControllerTestBase
    {
        [Test]
        public async Task IfNoRoleWithIdExistsThenCreateRoleIsCalled()
        {
            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((ApplicationRole) null);

            await Controller.EditOrCreatePost(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ));
            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Test]
        public async Task IfRoleExistsThenUpdateRoleIsCalled()
        {
            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((ApplicationRole)null);

            await Controller.EditOrCreatePost(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()));
            MockAdminDataContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Never);
        }

        [Test]
        public async Task IfConcurrencyStampIsInvalidThenReturnCurrentRole()
        {
            var dbModel = new ApplicationRole
            {
                Name = "Current Name",
                ConcurrencyStamp = "Current Stamp",
                Claims =
                {
                    new IdentityRoleClaim<string>
                    {
                        ClaimType = RoleClaimTypes.ForumsReader.ClaimType
                    }
                }
            };
            var modelToPost = new EditOrCreateRoleViewModel
            {
                Name = "Updated Name",
                ConcurrencyStamp = "Old Stamp",
                Permissions = new List<string>
                {
                    RoleClaimTypes.ForumsLocking.ClaimType
                }
            };

            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbModel);

            var result = await Controller.EditOrCreatePost(modelToPost);

            var viewModel = result.Model as EditOrCreateRoleViewModel;
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Name, Is.EqualTo(dbModel.Name));
            Assert.That(viewModel.ConcurrencyStamp, Is.EqualTo(dbModel.ConcurrencyStamp));
            // TODO: Verify permissions
        }
    }
}
