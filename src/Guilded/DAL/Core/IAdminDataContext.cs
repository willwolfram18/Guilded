using Guilded.ViewModels.Core;
using Guilded.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Guilded.Data.DAL.Core
{
    public interface IAdminDataContext : IReadWriteDataContext
    {
        #region Properties
        RoleManager<Identity.ApplicationRole> RoleManager { get; }

        IPermissionsRepository Permissions { get; }
        #endregion

        #region Methods
        Identity.ApplicationRole CreateRole(string roleName);
        
        IEnumerable<Permission> AddPermissionToRole(Identity.ApplicationRole role, Permission privilege);
        #endregion
    }
}
