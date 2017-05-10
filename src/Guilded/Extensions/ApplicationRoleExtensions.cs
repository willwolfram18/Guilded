using Guilded.Areas.Admin.ViewModels.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DataModel = Guilded.Identity.ApplicationRole;

namespace Guilded.Extensions
{
    public static class ApplicationRoleExtensions
    {
        public static void UpdateFromViewModel(this DataModel currentRole, ApplicationRoleViewModel viewModel)
        {
            if (currentRole == null)
            {
                throw new ArgumentNullException("currentRole");
            }

            currentRole.Name = viewModel.Name;
            //currentRole.ConcurrencyStamp = viewModel.ConcurrencyStamp;

            var roleClaims = NewRoleClaims(viewModel.Permissions, currentRole.Claims);
            currentRole.Claims.Clear();

            foreach (var identityRoleClaim in roleClaims)
            {
                identityRoleClaim.RoleId = currentRole.Id;
                currentRole.Claims.Add(identityRoleClaim);
            }
        }

        private static IEnumerable<IdentityRoleClaim<string>> NewRoleClaims(IEnumerable<Permission> newPermissions, IEnumerable<IdentityRoleClaim<string>> currentClaims)
        {
            var newSetOfClaimTypes = newPermissions.Select(p => p.PermissionType);
            var claimsToKeep = currentClaims.Where(c => newSetOfClaimTypes.Contains(c.ClaimType)).ToList();
            var claimsToAdd = newSetOfClaimTypes.Where(p => !claimsToKeep.Select(c => c.ClaimType).Contains(p));

            foreach (var claimType in claimsToAdd)
            {
                claimsToKeep.Add(new IdentityRoleClaim<string>
                {
                    ClaimType = claimType,
                    ClaimValue = "True"
                });
            }
            return claimsToKeep;
        }
    }
}
