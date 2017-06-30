using Guilded.DAL;
using Guilded.Data.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.DAL
{
    public interface IUsersDataContext : IReadWriteDataContext
    {
        IQueryable<ApplicationUser> GetUsers();

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<ApplicationUser> GetUserFromClaimsPrincipalAsync(ClaimsPrincipal user);

        Task<ApplicationRole> GetRoleForUserAsync(ApplicationUser user);

        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);

        Task<ApplicationUser> ChangeUserRoleAsync(ApplicationUser user, ApplicationRole newRole);

        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    }
}
