using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System;

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
                throw new ArgumentNullException("roleClaim");
            }

            PermissionType = roleClaim.ClaimType;
            Description = roleClaim.Description;
        }

        public Permission(IdentityRoleClaim<string> roleClaim) : this()
        {
            
            if (roleClaim == null)
            {
                throw new ArgumentNullException("privilege");
            }

            Id = roleClaim.Id;
            PermissionType = roleClaim.ClaimType;
            Description = RoleClaimTypes.LookUpGuildedRoleClaim(roleClaim).Description;
        }
        #endregion
    }
}