using System.Collections.Generic;
using System.Linq;
using Guilded.Data.DAL.Core;
using Guilded.Security.Claims;
using Guilded.Areas.Admin.ViewModels.Roles;
using Xunit;

namespace Guilded.Tests.Data.DAL.Core
{
    public class PermissionsRepositoryUnitTest
    {
        #region Properties
        private readonly PermissionsRepository _repo;
        #endregion

        public PermissionsRepositoryUnitTest()
        {
            _repo = new PermissionsRepository();
        }

        [Fact]
        public void Get_MatchesRoleClaims()
        {
            var repoPermissions = _repo.Get().OrderBy(p => p.PermissionType)
                .ToList();
            var securityPermissions = RoleClaimTypes.RoleClaims.OrderBy(p => p.ClaimType)
                .Select(p => new Permission(p)).ToList();

            Assert.Equal(repoPermissions.Count, securityPermissions.Count);
            for (int i = 0; i < repoPermissions.Count; i++)
            {
                Assert.Equal(repoPermissions[i].PermissionType, securityPermissions[i].PermissionType);
                Assert.Equal(repoPermissions[i].Description, securityPermissions[i].Description);
            }
        }
    }
}