using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Selama_SPA.Data.Models.Core;

namespace Selama_SPA.Data.DAL.Core
{
    public interface IPrivilegeReadWriteDataContext : IReadWriteDataContext
    {
        #region Properties
        UserManager<ApplicationUser> UserManager { get; }

        IPrivilegeReadOnlyRepository Privileges { get; }
        #endregion

        #region Methods
        ApplicationRole CreateRole(string roleName);

        IEnumerable<ResourcePrivilege> AddPrivilegeToRole(ApplicationRole role, ResourcePrivilege privilege);
        #endregion
    }
}
