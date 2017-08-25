using Guilded.Security.Claims;
using System;
using System.Security.Claims;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class Permission
    {
        #region Properties
        #region Public Properties
        public int Id { get; set; }

        public string PermissionType { get; set; }

        public string Description { get; set; }
        #endregion
        #endregion

        #region Constructors
        public Permission()
        {
        }

        public Permission(RoleClaim roleClaim) : this()
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }

            PermissionType = roleClaim.ClaimType;
            Description = roleClaim.Description;
        }

        public Permission(Claim roleClaim) : this()
        {
            
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }

            PermissionType = roleClaim.Type;
            Description = RoleClaimTypes.LookUpGuildedRoleClaim(roleClaim.Type).Description;
        }
        #endregion
    }
}