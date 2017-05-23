using Guilded.Data.DAL;
using Guilded.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.Data.DAL
{
    public interface IRolesDataContext : IReadWriteDataContext
    {
        IQueryable<ApplicationRole> GetRoles();

        ApplicationRole GetRoleById(string id);

        Task<ApplicationRole> GetRoleByName(string name);

        Task<ApplicationRole> CreateRoleAsync(ApplicationRole roleToCreate);

        Task<ApplicationRole> UpdateRoleAsync(ApplicationRole roleToUpdate);

        Task<IdentityResult> DeleteRole(ApplicationRole roleToDelete);
    }
}
