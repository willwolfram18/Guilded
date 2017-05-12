using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Tests.Areas.Admin.RolesControllerTests
{
    public class WhenEditOrCreateIsCalled : RolesControllerTestBase
    {
        [Test]
        public void IfRoleDoesNotExistThenNewRoleReturned([Values(null, "Failed id")] string roleId)
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
            var claims = new List<IdentityRoleClaim<string>>
            {
                new IdentityRoleClaim<string>
                {
                    ClaimType = RoleClaimTypes.ForumsLocking.ClaimType
                }
            };
            var dbRole = new ApplicationRole
            {
                Id = "Test id",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = "Test Role",
            };
            foreach (var claim in claims)
            {
                dbRole.Claims.Add(claim);
            }

            MockAdminDataContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns(dbRole);

            var result = Controller.EditOrCreate();

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.EqualTo(dbRole.Id));
            Assert.That(viewModel.Name, Is.EqualTo(dbRole.Name));
            Assert.That(viewModel.Permissions.Count, Is.EqualTo(claims.Count));

            for (var i = 0; i < claims.Count; i++)
            {
                Assert.That(viewModel.Permissions[i], Is.EqualTo(claims[i].ClaimType));
            }
        }

        [Test]
        public void ThenAvailablePermissionsInNameSortedOrder()
        {
            var result = Controller.EditOrCreate();

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AvailablePermissions.Count(), Is.EqualTo(RoleClaimTypes.RoleClaims.Count()));
            Assert.That(viewModel.AvailablePermissions, Is.Ordered.Ascending.By("Value"));
        }
    }
}
