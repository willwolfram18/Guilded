using Guilded.Data.ViewModels.Core;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Guilded.Data.DAL.Core
{
    public interface IManagerDataContext : IReadWriteDataContext
    {
        #region Properties
        RoleManager<Models.Core.ApplicationRole> RoleManager { get; }

        IPermissionsRepository Permissions { get; }
        #endregion

        #region Methods
        Models.Core.ApplicationRole CreateRole(string roleName);

        IEnumerable<Permission> AddPermissionToRole(Models.Core.ApplicationRole role, Permission privilege);
        #endregion
    }
}
