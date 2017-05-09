using Newtonsoft.Json;
using System;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class Permission
    {
        #region Properties
        #region Public Properties
        public string Id { get; set; }

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

            Mapper.Map(roleClaim, this);
        }

        public Permission(IdentityRoleClaim<string> roleClaim) : this()
        {
            
            if (roleClaim == null)
            {
                throw new ArgumentNullException("privilege");
            }

            Mapper.Map(roleClaim, this);
        }
        #endregion
    }
}