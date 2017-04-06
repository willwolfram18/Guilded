using Guilded.ViewModels.Core;
using Guilded.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.Data.DAL.Core
{
    public interface IAdminDataContext : IReadWriteDataContext
    {
        IPermissionsRepository Permissions { get; }

        IQueryable<Identity.ApplicationRole> GetRoles();

        Identity.ApplicationRole GetRoleById(string id);

        Identity.ApplicationRole CreateRole(string roleName, IEnumerable<Permission> permissions);

        Identity.ApplicationRole UpdateRole(Identity.ApplicationRole roleToUpdate);
    }
}
