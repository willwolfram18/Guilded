using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Data;
using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Guilded.Extensions
{
    public static class ApplicationDbContextExtensions
    {
        public static void EnsureRequiredDataIsPresent(this ApplicationDbContext context)
        {
            var adminRole = context.GetOrInsertRole("Admin");
            var guestRole = context.GetOrInsertRole("Guest");

            foreach (var roleClaim in RoleClaimValues.RoleClaims)
            {
                if (RoleDoesNotHaveClaim(context, adminRole, roleClaim))
                {
                    context.RoleClaims.Add(new IdentityRoleClaim<string>
                    {
                        ClaimValue = roleClaim.ClaimValue,
                        ClaimType = RoleClaimTypes.Permission,
                        RoleId = adminRole.Id
                    });
                }
            }

            if (RoleDoesNotHaveClaim(context, guestRole, RoleClaimValues.ForumsReader))
            {
                context.RoleClaims.Add(new IdentityRoleClaim<string>
                {
                    ClaimValue = RoleClaimValues.ForumsReaderClaim,
                    ClaimType = RoleClaimTypes.Permission,
                    RoleId = guestRole.Id
                });
            }

            context.SaveChanges();
        }

        private static ApplicationRole GetOrInsertRole(this ApplicationDbContext context, string roleName)
        {
            var role = context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

            if (role == null)
            {
                role = new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                context.Roles.Add(role);
                context.SaveChanges();
                context.Entry(role).Reload();
            }

            return role;
        }

        private static bool RoleDoesNotHaveClaim(ApplicationDbContext context, ApplicationRole adminRole, RoleClaim roleClaim)
        {
            return !context.RoleClaims.Any(rc =>
                rc.ClaimType == RoleClaimTypes.Permission && rc.ClaimValue == roleClaim.ClaimValue &&
                rc.RoleId == adminRole.Id
            );
        }
    }
}
