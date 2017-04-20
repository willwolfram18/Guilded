using Guilded.ViewModels.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Data.DAL.Core
{
    public interface IAdminDataContext : IReadWriteDataContext
    {
        IQueryable<Identity.ApplicationRole> GetRoles();

        Identity.ApplicationRole GetRoleById(string id);

        Task<Identity.ApplicationRole> CreateRoleAsync(string roleName, IEnumerable<Permission> permissions);

        Task<Identity.ApplicationRole> UpdateRoleAsync(Identity.ApplicationRole roleToUpdate);

        void DeleteRole(string roleId);

        IEnumerable<Permission> GetPermissions();
    }
}
