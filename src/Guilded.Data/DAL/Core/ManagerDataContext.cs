using Guilded.Data.DAL.Abstract;
using Guilded.Data.ViewModels.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Guilded.Data.DAL.Core
{
    public class ManagerDataContext : ReadWriteDataContextBase, IManagerDataContext
    {
        #region Properties
        #region Public Properties
        public RoleManager<Models.Core.ApplicationRole> RoleManager => _roleManager;

        public IPermissionsRepository Permissions => _privileges;
        #endregion

        #region Private Properties
        private readonly RoleManager<Models.Core.ApplicationRole> _roleManager;
        private readonly IPermissionsRepository _privileges;
        #endregion
        #endregion
        

        public ManagerDataContext(ApplicationDbContext context, RoleManager<Models.Core.ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
            _privileges = new PermissionsRepository();
        }

        public Models.Core.ApplicationRole CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Permission> AddPermissionToRole(Models.Core.ApplicationRole role, Permission privilege)
        {
            throw new NotImplementedException();
        }
    }
}