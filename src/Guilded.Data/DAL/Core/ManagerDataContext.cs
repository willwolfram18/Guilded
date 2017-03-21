using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Guilded.Data.DAL.Abstract;
using Guilded.Data.Models.Core;

namespace Guilded.Data.DAL.Core
{
    public class ManagerDataContext : ReadWriteDataContextBase, IManagerDataContext
    {
        #region Properties
        #region Public Properties
        public RoleManager<ApplicationRole> RoleManager => _roleManager;

        public IPermissionsRepository Permissions => _privileges;
        #endregion

        #region Private Properties
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionsRepository _privileges;
        #endregion
        #endregion
        

        public ManagerDataContext(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
            _privileges = new PermissionsRepository(_context);
        }

        public IEnumerable<ResourcePrivilege> AddPermissionToRole(ApplicationRole role, ResourcePrivilege privilege)
        {
            throw new NotImplementedException();
        }

        public ApplicationRole CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}