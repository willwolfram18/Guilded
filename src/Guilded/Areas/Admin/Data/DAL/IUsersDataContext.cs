using Guilded.Data.DAL;
using Guilded.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.Data.DAL
{
    public interface IUsersDataContext : IReadWriteDataContext
    {
        IQueryable<ApplicationUser> GetUsers();

        Task<ApplicationUser> GetUserById(string id);
    }
}
