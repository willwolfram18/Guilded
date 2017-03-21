using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Guilded.Data.Models.Core;

namespace Guilded.Data.DAL.Core
{
    public interface IManagerDataContext : IReadWriteDataContext
    {
        #region Properties
        RoleManager<ApplicationRole> RoleManager { get; }

        IPermissionsRepository Permissions { get; }
        #endregion

        #region Methods
        ApplicationRole CreateRole(string roleName);

        IEnumerable<ResourcePrivilege> AddPermissionToRole(ApplicationRole role, ResourcePrivilege privilege);
        #endregion
    }
}
