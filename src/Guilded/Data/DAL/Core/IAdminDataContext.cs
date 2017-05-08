using Guilded.Areas.Admin.ViewModels.Roles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Guilded.Data.DAL.Core
{
    public interface IAdminDataContext : IReadWriteDataContext
    {
        IQueryable<Identity.ApplicationRole> GetRoles();

        Identity.ApplicationRole GetRoleById(string id);

        Task<Identity.ApplicationRole> CreateRoleAsync(string roleName, IEnumerable<Permission> permissions);

        Task<Identity.ApplicationRole> UpdateRoleAsync(Identity.ApplicationRole roleToUpdate);

        Task<IdentityResult> DeleteRole(Identity.ApplicationRole roleToDelete);

        IEnumerable<Permission> GetPermissions();
    }
}
