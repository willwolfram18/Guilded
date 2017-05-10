using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Moq;
using NUnit.Framework;

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
            var result = await Controller.EditOrCreatePost(null);
        }
    }
}
