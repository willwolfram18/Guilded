using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Admin.RolesControllerTests
{
    public class WhenEditOrCreatePostIsCalled : RolesControllerTest
    {
        [Test]
        public async Task IfRoleDoesNotExistThenCreateRoleIsCalled()
        {
            MockAdminDataContext.Setup(db => db.GetRoleByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationRole) null);
            MockAdminDataContext.Setup(db => db.CreateRoleAsync(It.IsAny<ApplicationRole>())).ReturnsAsync(new ApplicationRole());

            await Controller.EditOrCreate(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.CreateRoleAsync(It.IsAny<ApplicationRole>()));
            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }

        [Test]
        public async Task IfRoleDoesExistThenUpdateRoleIsCalled()
        {
            var dbRole = new ApplicationRole();
            MockAdminDataContext.Setup(db => db.GetRoleByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(dbRole);
            MockAdminDataContext.Setup(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(new ApplicationRole());

            await Controller.EditOrCreate(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()));
            MockAdminDataContext.Verify(db => db.CreateRoleAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }
    }
}
