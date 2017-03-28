using Guilded.Data.DAL.Abstract;
using Guilded.ViewModels.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Guilded.Data.DAL.Core
{
    public class AdminDataContext : ReadWriteDataContextBase, IAdminDataContext
    {
        #region Properties
        #region Public Properties
        public RoleManager<Identity.ApplicationRole> RoleManager => _roleManager;

        public IPermissionsRepository Permissions => _privileges;
        #endregion

        #region Private Properties
        private readonly RoleManager<Identity.ApplicationRole> _roleManager;
        private readonly IPermissionsRepository _privileges;
        #endregion
        #endregion
        

        public AdminDataContext(ApplicationDbContext context, RoleManager<Identity.ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
            _privileges = new PermissionsRepository();
        }

        public Identity.ApplicationRole CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Permission> AddPermissionToRole(Identity.ApplicationRole role, Permission privilege)
        {
            throw new NotImplementedException();
        }
    }
}