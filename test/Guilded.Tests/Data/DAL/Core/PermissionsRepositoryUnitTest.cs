using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Guilded.AutoMapper;
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
            Mappings.Initialize();
            _repo = new PermissionsRepository();
        }

        [Fact]
        public void Get_MatchesRoleClaims()
        {
            var repoPermissions = _repo.Get().OrderBy(p => p.PermissionType)
                .ToList();
            var securityPermissions = Mapper.Map<IList<Permission>>(RoleClaimTypes.RoleClaims.OrderBy(p => p.ClaimType));

            Assert.Equal(repoPermissions.Count, securityPermissions.Count);
            for (int i = 0; i < repoPermissions.Count; i++)
            {
                Assert.Equal(repoPermissions[i].PermissionType, securityPermissions[i].PermissionType);
                Assert.Equal(repoPermissions[i].Description, securityPermissions[i].Description);
            }
        }
    }
}