using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Identity;

namespace Guilded.Data.DAL.Core
{
    public interface IAdminDataContext : IReadWriteDataContext
    {
        IQueryable<ApplicationRole> GetRoles();

        ApplicationRole GetRoleById(string id);

        Task<ApplicationRole> CreateRoleAsync(ApplicationRole roleToCreate);

        Task<ApplicationRole> UpdateRoleAsync(ApplicationRole roleToUpdate);

        Task<IdentityResult> DeleteRole(ApplicationRole roleToDelete);

        IEnumerable<Permission> GetPermissions();
    }
}
