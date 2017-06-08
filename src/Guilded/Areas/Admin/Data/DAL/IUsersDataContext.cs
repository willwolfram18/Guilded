using Guilded.Data.DAL;
using Guilded.Data.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.Data.DAL
{
    public interface IUsersDataContext : IReadWriteDataContext
    {
        IQueryable<ApplicationUser> GetUsers();

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<ApplicationRole> GetRoleForUserAsync(ApplicationUser user);

        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
    }
}
