using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guilded.Data.ViewModels.Core
{
    public class Permission
    {
        #region Properties
        #region Public properties
        public string PermissionType { get; set; }

        public string PermissionValue { get; set; }
        #endregion
        #endregion

        public Permission()
        {
        }

        public Permission(IdentityRoleClaim<string> roleClaim)
        {
            PermissionType = roleClaim.ClaimType;
            PermissionValue = roleClaim.ClaimValue;
        }
    }
}
