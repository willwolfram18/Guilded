using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Guilded.Tests.Controllers.Admin.RolesControllerUnitTests
{
    public class WhenDeleteIsCalled : RolesControllerUnitTestBase
    {
        [Fact]
        public async Task IfRoleDoesntExistThenDeleteIsntCalled()
        {
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((ApplicationRole)null);

            var result = await Controller.Delete("");

            Assert.IsType<NotFoundResult>(result);
            _mockAdminContext.Verify(db => db.DeleteRole(It.IsAny<ApplicationRole>()), Times.Never());
        }

        [Fact]
        public async Task IfDeleteFailsThenBadRequest()
        {
            DbRoleExists();
            _mockAdminContext.Setup(db => db.DeleteRole(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await Controller.Delete("");

            Assert.IsType<BadRequestResult>(result);
            _mockAdminContext.Verify(db => db.DeleteRole(It.IsAny<ApplicationRole>()));
        }

        [Fact]
        public async Task IfRoleExistsThenDeleteIsCalled()
        {
            DbRoleExists();
            _mockAdminContext.Setup(db => db.DeleteRole(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await Controller.Delete("");

            Assert.IsType<OkResult>(result);
            _mockAdminContext.Verify(db => db.DeleteRole(It.IsAny<ApplicationRole>()));
        }

        private void DbRoleExists()
        {
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(new ApplicationRole()
                {
                    Id = "RoleId",
                    Name = "Role to delete"
                });
        }
    }
}
