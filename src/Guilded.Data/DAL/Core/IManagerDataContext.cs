using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Guilded.Data.Models.Core;
using Guilded.Data.ViewModels.Core;

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

        IEnumerable<ResourcePrivilege> AddPermissionToRole(Models.Core.ApplicationRole role, Permission privilege);
        #endregion
    }
}
