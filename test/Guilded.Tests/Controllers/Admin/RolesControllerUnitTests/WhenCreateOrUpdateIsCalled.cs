using Guilded.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Identity;
using Guilded.Areas.Admin.ViewModels.Roles;
using Moq;
using Xunit;
using DataModel = Guilded.Identity.ApplicationRole;
using ViewModel = Guilded.Areas.Admin.ViewModels.Roles.ApplicationRole;

namespace Guilded.Tests.Controllers.Admin.RolesControllerUnitTests
{
    public class WhenCreateOrUpdateIsCalled : RolesControllerUnitTestBase
    {
        [Fact]
        public async Task IfRoleIdDoesntExistThenCreateNewRole()
        {
            ViewModel roleToCreate = new ViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "New Role",
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>())).Returns((DataModel) null);
            _mockAdminContext.Setup(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            )).ReturnsAsync(new DataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = roleToCreate.Name
            });

            var result = await Controller.CreateOrUpdate(roleToCreate);

            var createdRole = AssertResultIsRoleViewModel(result);
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.Is<string>(str => str == roleToCreate.Name),
                It.IsAny<IEnumerable<Permission>>()
            ));
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(It.IsAny<DataModel>()), Times.Never());
        }

        [Fact]
        public async Task IfRoleIdExistsThenUpdateRole()
        {
            DataModel dbRole = new DataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Existing Role",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);
            _mockAdminContext.Setup(db => db.UpdateRoleAsync(It.IsAny<DataModel>()))
                .Returns((Func<DataModel, Task<DataModel>>)(role => Task.FromResult(role)));
            ViewModel roleToUpdate = new ViewModel(dbRole);
            roleToUpdate.Name = "Updated Role Name";

            var result = await Controller.CreateOrUpdate(roleToUpdate);

            var updatedRole = AssertResultIsRoleViewModel(result);
            Assert.Equal(roleToUpdate.Name, updatedRole.Name);
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Never());
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(
                It.Is<DataModel>(role => role.Id == roleToUpdate.Id)
            ));
        }

        [Fact]
        public async Task IfConcurrencyStampIsInvalidThenReturnCurrentRole()
        {
            DataModel dbRole = new DataModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Existing Role",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);
            ViewModel roleToUpdate = new ViewModel
            {
                Id = dbRole.Id,
                Name = "Updated Role Name",
                ConcurrencyStamp = Guid.Empty.ToString(),
            };

            var result = await Controller.CreateOrUpdate(roleToUpdate);

            var currentRole = AssertResultIsRoleViewModel(result);
            Assert.Equal(dbRole.Name, currentRole.Name);
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Never());
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(It.IsAny<DataModel>()), Times.Never());
        }
    }
}
