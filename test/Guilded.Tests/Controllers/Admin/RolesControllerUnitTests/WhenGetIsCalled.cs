using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DataModel = Guilded.Identity.ApplicationRole;
using ViewModel = Guilded.Areas.Admin.ViewModels.Roles.ApplicationRole;

namespace Guilded.Tests.Controllers.Admin.RolesControllerUnitTests
{
    public class WhenGetIsCalled : RolesControllerUnitTestBase
    {
        [Fact]
        public void IsCalledWithoutIdThenListOfRolesIsReturned()
        {
            var roles = new List<DataModel>();
            for (int i = 1; i <= 5; i++)
            {
                roles.Add(new DataModel
                {
                    Id = i.ToString(),
                    Name = "Role " + i.ToString(),
                });
            }
            _mockAdminContext.Setup(db => db.GetRoles()).Returns(roles.AsQueryable());

            var result = Controller.Get();

            var resultRoles = result.Value as List<ViewModel>;
            Assert.NotNull(resultRoles);
            Assert.Equal(roles.Count, resultRoles.Count);
            for (int i = 0; i < roles.Count; i++)
            {
                Assert.Equal(roles[i].Id, resultRoles[i].Id);
                Assert.Equal(roles[i].Name, resultRoles[i].Name);
            }
        }

        [Fact]
        public async Task IsCalledWithInvalidIdThenNewRoleIsReturned()
        {
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((DataModel)null);

            var result = await Controller.Get("");

            var role = result.Value as ViewModel;
            Assert.NotNull(role);
            Assert.Null(role.Id);
            Assert.Null(role.Name);
            Assert.NotNull(role.ConcurrencyStamp);
            Assert.Empty(role.Permissions);
        }

        [Fact]
        public async Task IsCalledWithValidIdThenRoleIsReturned()
        {
            string roleId = Guid.NewGuid().ToString();
            var expectedRole = new DataModel
            {
                Id = roleId,
                Name = "Sample Role",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Claims =
                {
                    new IdentityRoleClaim<string>()
                    {
                        ClaimType = RoleClaimTypes.ForumsLocking.ClaimType,
                        ClaimValue = "True",
                        RoleId = roleId,
                    },
                    new IdentityRoleClaim<string>()
                    {
                        ClaimType = RoleClaimTypes.RoleManagement.ClaimType,
                        ClaimValue = "True",
                        RoleId = roleId,
                    }
                }
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(expectedRole);

            var result = await Controller.Get(roleId);

            var role = result.Value as ViewModel;
            Assert.NotNull(role);
            Assert.Equal(expectedRole.Id, role.Id);
            Assert.Equal(expectedRole.Name, role.Name);
        }
    }
}
