using Newtonsoft.Json;
using System;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Guilded.ViewModels.Core
{
    public class Permission
    {
        #region Properties
        #region Public Properties
        [JsonProperty("permissionType")]
        public string PermissionType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        #endregion
        #endregion

        #region Constructors
        public Permission()
        {
        }

        public Permission(RoleClaim roleClaim)
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException("roleClaim");
            }

            PermissionType = roleClaim.ClaimType;
            Description = roleClaim.Description;
        }

        public Permission(IdentityRoleClaim<string> roleClaim)
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException("privilege");
            }

            RoleClaim rc = RoleClaimTypes.LookUpRoleClaimByIdentityRoleClaim(roleClaim);
            PermissionType = rc.ClaimType;
            Description = rc.Description;
        }
        #endregion
    }
}