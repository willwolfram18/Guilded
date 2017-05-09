using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Moq;
using System;
using Xunit;

namespace Guilded.Tests.Areas.Admin.RolesController
{
    public class WhenEditOrCreateIsCalled : RolesControllerTestBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData("Failed Id")]
        public void IfRoleDoesntExistThenNewRoleReturned(string roleId)
        {
            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((Identity.ApplicationRole)null);

            var result = Controller.EditOrCreate(roleId);

            var viewModel = result.Model as ApplicationRoleViewModel;

            Assert.NotNull(viewModel);
            Assert.NotEqual(viewModel.Id, roleId);
            Assert.NotNull(viewModel.ConcurrencyStamp);
        }

        [Fact]
        public void ThenGetRoleByIdIsCalled()
        {
            var result = Controller.EditOrCreate();

            MockAdminDataContext.Verify(db => db.GetRoleById(It.IsAny<string>()));
        }

        [Fact]
        public void ThenViewModelMatchesDataModel()
        {
            var dbRole = new ApplicationRole
            {
                Id = "Test id",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = "Test Role"
            };

            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);

            var result = Controller.EditOrCreate();

            var viewModel = result.Model as ApplicationRoleViewModel;

            Assert.NotNull(viewModel);
            Assert.Equal(dbRole.Id, viewModel.Id);
            Assert.Equal(dbRole.ConcurrencyStamp, viewModel.ConcurrencyStamp);
            Assert.Equal(dbRole.Name, viewModel.Name);
        }
    }
}
