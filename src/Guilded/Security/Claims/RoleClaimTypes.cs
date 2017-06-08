using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Guilded.Security.Claims
{
    public static class RoleClaimTypes
    {
        public const string RoleManagementClaim = "Guilded:Admin:Roles";
        public const string ForumsPinningClaim = "Guilded:Forums:Pin Posts";
        public const string ForumsLockingClaim = "Guilded:Forums:Lock Posts";
        public const string ForumsReaderClaim = "Guilded:Forums:Read Posts";
        public const string ForumsWriterClaim = "Guilded:Forums:Write Posts";

        public static readonly RoleClaim RoleManagement = new RoleClaim(
            RoleManagementClaim,
            "Permission to create, edit, and apply roles"
        );

        public static readonly RoleClaim ForumsPinning = new RoleClaim
        (
            ForumsPinningClaim,
            "Permission to pin posts in the forums"
        );
        public static readonly RoleClaim ForumsLocking = new RoleClaim
        (
            ForumsLockingClaim,
            "Permission to lock posts in the forums"
        );
        public static readonly RoleClaim ForumsReader = new RoleClaim
        (
            ForumsReaderClaim,
            "Permission to read posts in the forums"
        );
        public static readonly RoleClaim ForumsWriter = new RoleClaim
        (
            ForumsWriterClaim,
            "Permission to create and reply to posts in the forums"
        );

        public static readonly IEnumerable<RoleClaim> RoleClaims = new List<RoleClaim>
        {
            RoleManagement,
            ForumsPinning,
            ForumsLocking,
            ForumsReader,
            ForumsWriter,
        };

        private static Dictionary<string, RoleClaim> _typesToRolesClaims;
        private static Dictionary<string, RoleClaim> TypesToRoleClaims
        {
            get
            {
                if (_typesToRolesClaims == null)
                {
                    _typesToRolesClaims = new Dictionary<string, RoleClaim>();
                    foreach (RoleClaim rc in RoleClaims)
                    {
                        _typesToRolesClaims.Add(rc.ClaimType, rc);
                    }
                }
                return _typesToRolesClaims;
            }
        }

        public static RoleClaim LookUpGuildedRoleClaim(IdentityRoleClaim<string> identityRoleClaim)
        {
            return LookUpGuildedRoleClaim(identityRoleClaim.ClaimType);
        }

        public static RoleClaim LookUpGuildedRoleClaim(string claimType)
        {
            if (!TypesToRoleClaims.ContainsKey(claimType))
            {
                throw new KeyNotFoundException($"Unable to fund role claim {claimType}");
            }

            return TypesToRoleClaims[claimType];
        }
    }
}
