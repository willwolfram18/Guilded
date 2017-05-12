using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Constants;

namespace Guilded.Tests.Areas.Admin.RolesControllerTests
{
    public class WhenEditOrCreatePostIsCalled : RolesControllerTestBase
    {
        [Test]
        public async Task IfRoleDoesNotExistThenCreateRoleIsCalled()
        {
            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((ApplicationRole) null);
            MockAdminDataContext.Setup(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleClaim>>()
            )).ReturnsAsync(new ApplicationRole());

            await Controller.EditOrCreate(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleClaim>>()
            ));
            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Test]
        public async Task IfRoleDoesExistThenUpdateRoleIsCalled()
        {
            var dbRole = new ApplicationRole();
            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);
            MockAdminDataContext.Setup(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(new ApplicationRole());

            await Controller.EditOrCreate(new EditOrCreateRoleViewModel
            {
                ConcurrencyStamp = dbRole.ConcurrencyStamp
            });

            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()));
            MockAdminDataContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleClaim>>()
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
                    },
                    new IdentityRoleClaim<string>
                    {
                        ClaimType = RoleClaimTypes.ForumsWriter.ClaimType
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

            var result = await Controller.EditOrCreate(modelToPost);

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Name, Is.EqualTo(dbModel.Name));
            Assert.That(viewModel.ConcurrencyStamp, Is.EqualTo(dbModel.ConcurrencyStamp));
            Assert.That(viewModel.Permissions.Count, Is.EqualTo(dbModel.Claims.Count));

            for (var i = 0; i < dbModel.Claims.Count; i++)
            {
                Assert.That(viewModel.Permissions[i], Is.EqualTo(dbModel.Claims.ElementAt(i).ClaimType));
            }
        }

        [Test]
        public async Task IfConcurrencyStampIsInvalidThenCreateAndUpdateAreNotCalled()
        {
            var dbModel = new ApplicationRole
            {
                ConcurrencyStamp = "New stamp"
            };
            var modelToPost = new EditOrCreateRoleViewModel
            {
                ConcurrencyStamp = "Old stamp"
            };

            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbModel);

            await Controller.EditOrCreate(modelToPost);

            MockAdminDataContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleClaim>>()
            ), Times.Never);
            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Test]
        public async Task IfConcurrencyStampIsInvalidThenViewDataErrorMessagesIsNotNull()
        {
            var dbModel = new ApplicationRole
            {
                ConcurrencyStamp = "New stamp"
            };
            var modelToPost = new EditOrCreateRoleViewModel
            {
                ConcurrencyStamp = "Old stamp"
            };

            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbModel);

            var result = await Controller.EditOrCreate(modelToPost);

            Assert.That(result.ViewData[ViewDataKeys.ErrorMessages]?.ToString(), Is.Not.Empty);
        }
    }
}
