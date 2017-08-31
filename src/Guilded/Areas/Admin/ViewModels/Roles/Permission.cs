using Guilded.Security.Claims;
using System;
using System.Security.Claims;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class Permission
    {
        public string PermissionValue { get; set; }

        public string Description { get; set; }

        public Permission()
        {
        }

        public Permission(RoleClaim roleClaim) : this()
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }

            PermissionValue = roleClaim.ClaimValue;
            Description = roleClaim.Description;
        }

        public Permission(Claim roleClaim) : this()
        {
            
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }

            PermissionValue = roleClaim.Value;
            Description = RoleClaimValues.LookUpGuildedRoleClaim(roleClaim.Value).Description;
        }
    }
}