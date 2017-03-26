using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guilded.Security.Claims
{
    public static class RoleClaimTypes
    {
        #region Properties
        #region Public properties
        public static readonly RoleClaim RoleManagement = new RoleClaim(
            "Guilded:Admin:Roles",
            "Permission to create, edit, and apply roles"
        );

        public static readonly RoleClaim ForumsPinning = new RoleClaim
        (
            "Guilded:Forums:Pin Posts",
            "Permission to pin posts in the forums"
        );
        public static readonly RoleClaim ForumsLocking = new RoleClaim
        (
            "Guilded:Forums:Lock Posts",
            "Permission to lock posts in the forums"
        );
        public static readonly RoleClaim ForumsReader = new RoleClaim
        (
            "Guilded:Forums:Read Posts",
            "Permission to read posts in the forums"
        );
        public static readonly RoleClaim ForumsWriter = new RoleClaim
        (
            "Guilded:Forums:Write Posts",
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
        #endregion

        #region Private properties
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

        private static Dictionary<string, RoleClaim> _typesToRolesClaims;
        #endregion
        #endregion

        #region Methods
        #region Public methods
        public static RoleClaim LookUpRoleClaimByIdentityRoleClaim(IdentityRoleClaim<string> identityRoleClaim)
        {
            if (!TypesToRoleClaims.ContainsKey(identityRoleClaim.ClaimType))
            {
                throw new KeyNotFoundException($"Unable to fined role claim {identityRoleClaim.ClaimType}");
            }
            return TypesToRoleClaims[identityRoleClaim.ClaimType];
        }
        #endregion
        #endregion
    }
}
