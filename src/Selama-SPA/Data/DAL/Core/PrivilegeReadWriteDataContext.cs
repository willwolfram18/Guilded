using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Selama_SPA.Data.DAL.Abstract;
using Selama_SPA.Data.Models.Core;

namespace Selama_SPA.Data.DAL.Core
{
    public class PrivilegeReadWriteDataContext : ReadWriteDataContextBase, IPrivilegeReadWriteDataContext
    {
        #region Properties
        #region Public Properties
        public UserManager<ApplicationUser> UserManager => _userManager;

        public IPrivilegeReadOnlyRepository Privileges => _privileges;
        #endregion

        #region Private Properties
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPrivilegeReadOnlyRepository _privileges;
        #endregion
        #endregion
        

        public PrivilegeReadWriteDataContext(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
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