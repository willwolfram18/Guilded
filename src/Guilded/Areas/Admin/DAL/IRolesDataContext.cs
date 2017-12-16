using System.Collections.Generic;
using Guilded.DAL;
using Guilded.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guilded.Security.Claims;

namespace Guilded.Areas.Admin.DAL
{
    public interface IRolesDataContext : IReadWriteDataContext
    {
        IQueryable<ApplicationRole> GetRoles();

        Task<ApplicationRole> GetRoleByIdAsync(string id);

        Task<ApplicationRole> CreateRoleAsync(ApplicationRole roleToCreate, IEnumerable<RoleClaim> roleClaims);

        Task<ApplicationRole> UpdateRoleAsync(ApplicationRole roleToUpdate, IEnumerable<RoleClaim> roleClaims);

        Task<IdentityResult> DeleteRoleAsync(ApplicationRole roleToDelete);

        IEnumerable<Claim> GetClaimsForRole(ApplicationRole role);
    }
}
