using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Tests.Areas.Admin.RolesControllerTests
{
    public class WhenEditOrCreateIsCalled : RolesControllerTest
    {
        [Test]
        public async Task IfRoleDoesNotExistThenNewRoleReturned([Values(null, "Failed id")] string roleId)
        {
            MockAdminDataContext.Setup(db => db.GetRoleByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationRole)null);

            var result = await Controller.EditOrCreate(roleId);

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.Not.EqualTo(roleId));
        }

        [Test]
        public async Task ThenGetRoleByIdIsCalled()
        {
            await Controller.EditOrCreate();

            MockAdminDataContext.Verify(db => db.GetRoleByIdAsync(It.IsAny<string>()));
        }

        [Test]
        public async Task ThenViewModelMatchesDataModel()
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

            MockAdminDataContext.Setup(db => db.GetRoleByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(dbRole);

            var result = await Controller.EditOrCreate();

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
        public async Task ThenAvailablePermissionsInNameSortedOrder()
        {
            var result = await Controller.EditOrCreate();

            var viewModel = result.Model as EditOrCreateRoleViewModel;

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AvailablePermissions.Count(), Is.EqualTo(RoleClaimTypes.RoleClaims.Count()));
            Assert.That(viewModel.AvailablePermissions, Is.Ordered.Ascending.By("Value"));
        }
    }
}
