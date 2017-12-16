using System.Collections.Generic;

namespace Guilded.Security.Claims
{
    public static class RoleClaimValues
    {
        public const string RoleManagementClaim = "guilded.admin.roles";
        public const string UserManagementClaim = "guilded.admin.users";
        public const string ForumsPinningClaim = "guilded.forums.pin-posts";
        public const string ForumsLockingClaim = "guilded.forums.lock-posts";
        public const string ForumsReaderClaim = "guilded.forums.read-posts";
        public const string ForumsWriterClaim = "guilded.forums.write-posts";

        public static readonly RoleClaim RoleManagement = new RoleClaim(
            RoleManagementClaim,
            "Permission to create, edit, and apply roles"
        );
        public static readonly RoleClaim UserManagement = new RoleClaim(
            UserManagementClaim,
            "Permission to enable or disable users, and manage their website role"
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
            UserManagement,
            ForumsPinning,
            ForumsLocking,
            ForumsReader,
            ForumsWriter,
        }.AsReadOnly();

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
                        _typesToRolesClaims.Add(rc.ClaimValue, rc);
                    }
                }
                return _typesToRolesClaims;
            }
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
