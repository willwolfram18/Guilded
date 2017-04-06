using Guilded.Data.DAL.Abstract;
using Guilded.ViewModels.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Data.DAL.Core
{
    public class AdminDataContext : ReadWriteDataContextBase, IAdminDataContext
    {
        #region Properties
        #region Public properties
        public IPermissionsRepository Permissions => _permissions;
        #endregion

        #region Private Properties
        private readonly RoleManager<Identity.ApplicationRole> _roleManager;
        private readonly IPermissionsRepository _permissions;
        #endregion
        #endregion
        

        public AdminDataContext(ApplicationDbContext context,
            RoleManager<Identity.ApplicationRole> roleManager,
            IPermissionsRepository permissions) : base(context)
        {
            _roleManager = roleManager;
            _permissions = permissions;
        }

        public IQueryable<Identity.ApplicationRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public Identity.ApplicationRole GetRoleById(string id)
        {
            return _roleManager.Roles.FirstOrDefault(r => r.Id == id);
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