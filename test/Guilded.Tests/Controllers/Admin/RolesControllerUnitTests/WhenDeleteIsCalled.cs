using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Guilded.Tests.Controllers.Admin.RolesControllerUnitTests
{
    public class WhenDeleteIsCalled : RolesControllerUnitTestBase
    {
        [Fact]
        public void IfRoleDoesntExistThenDeleteIsntCalled()
        {
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((ApplicationRole)null);

            var result = Controller.Delete("");

            Assert.IsType<NotFoundResult>(result);
            _mockAdminContext.Verify(db => db.DeleteRole(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void IfRoleExistsThenDeleteIsCalled()
        {
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(new ApplicationRole()
                {
                    Id = "RoleId",
                    Name = "Role to delete"
                });

            var result = Controller.Delete("");

            Assert.IsType<OkResult>(result);
            _mockAdminContext.Verify(db => db.DeleteRole(It.IsAny<string>()));
        }
    }
}
