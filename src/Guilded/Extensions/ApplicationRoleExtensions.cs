using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Extensions
{
    public static class ApplicationRoleExtensions
    {
        public static bool HasRoleClaim(this ApplicationRole role, RoleClaim roleClaim)
        {
            return role.Claims.Any(c => c.ClaimType == roleClaim.ClaimType);
        }

        public static void UpdateFromViewModel(this ApplicationRole currentRole, EditOrCreateRoleViewModel viewModel)
        {
            if (currentRole == null)
            {
                throw new ArgumentNullException(nameof(currentRole));
            }

            currentRole.Name = viewModel.Name;

            var roleClaims = NewRoleClaims(viewModel.PermissionsAsRoleClaims, currentRole.Claims);
            currentRole.Claims.Clear();

            foreach (var identityRoleClaim in roleClaims)
            {
                identityRoleClaim.RoleId = currentRole.Id;
                currentRole.Claims.Add(identityRoleClaim);
            }
        }

        private static IEnumerable<IdentityRoleClaim<string>> NewRoleClaims(IEnumerable<RoleClaim> newPermissions, IEnumerable<IdentityRoleClaim<string>> currentClaims)
        {
            var permissionsForRole = newPermissions.Select(rc => rc.ClaimType);
            var permissionsToKeep = currentClaims
                .Where(c => permissionsForRole.Contains(c.ClaimType))
                .ToList();
            var newPermissionsForRole = permissionsForRole.Where(p => !permissionsToKeep.Select(c => c.ClaimType).Contains(p));

            foreach (var claimType in newPermissionsForRole)
            {
                permissionsToKeep.Add(new IdentityRoleClaim<string>
                {
                    ClaimType = claimType,
                    ClaimValue = "True"
                });
            }
            return permissionsToKeep;
        }
    }
}
