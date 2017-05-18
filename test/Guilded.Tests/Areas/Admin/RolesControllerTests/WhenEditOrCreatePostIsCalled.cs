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
            MockAdminDataContext.Setup(db => db.CreateRoleAsync(It.IsAny<ApplicationRole>())).ReturnsAsync(new ApplicationRole());

            await Controller.EditOrCreate(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.CreateRoleAsync(It.IsAny<ApplicationRole>()));
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

            await Controller.EditOrCreate(new EditOrCreateRoleViewModel());

            MockAdminDataContext.Verify(db => db.UpdateRoleAsync(It.IsAny<ApplicationRole>()));
            MockAdminDataContext.Verify(db => db.CreateRoleAsync(It.IsAny<ApplicationRole>()), Times.Never);
        }
    }
}
