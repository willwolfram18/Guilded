using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Guilded.Data.DAL.Abstract;
using Guilded.Data.Models.Core;

namespace Guilded.Data.DAL.Core
{
    public class PrivilegeReadWriteDataContext : ReadWriteDataContextBase, IPrivilegeReadWriteDataContext
    {
        #region Properties
        #region Public Properties
        public RoleManager<ApplicationRole> RoleManager => _roleManager;

        public IPrivilegeReadOnlyRepository Privileges => _privileges;
        #endregion

        #region Private Properties
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPrivilegeReadOnlyRepository _privileges;
        #endregion
        #endregion
        

        public PrivilegeReadWriteDataContext(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
            _privileges = new PrivilegeReadOnlyRepository(_context);
        }

        public IEnumerable<ResourcePrivilege> AddPrivilegeToRole(ApplicationRole role, ResourcePrivilege privilege)
        {
            throw new NotImplementedException();
        }

        public ApplicationRole CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}