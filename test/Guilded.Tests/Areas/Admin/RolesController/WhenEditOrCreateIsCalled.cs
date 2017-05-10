using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Moq;
using NUnit.Framework;
using System;

namespace Guilded.Tests.Areas.Admin.RolesController
{
    public class WhenEditOrCreateIsCalled : RolesControllerTestBase
    {
        [Test]
        public void IfRoleDoesntExistThenNewRoleReturned([Values(null, "Failed id")] string roleId)
        {
            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((ApplicationRole)null);

            var result = Controller.EditOrCreate(roleId);

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.Not.EqualTo(roleId));
        }

        [Test]
        public void ThenGetRoleByIdIsCalled()
        {
            Controller.EditOrCreate();

            MockAdminDataContext.Verify(db => db.GetRoleById(It.IsAny<string>()));
        }

        [Test]
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

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.EqualTo(dbRole.Id));
            Assert.That(viewModel.Name, Is.EqualTo(dbRole.Name));
        }
    }
}
