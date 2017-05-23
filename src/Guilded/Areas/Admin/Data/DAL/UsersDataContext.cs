using Guilded.Data;
using Guilded.Data.DAL.Abstract;
using Guilded.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.Data.DAL
{
    public class UsersDataContext : ReadWriteDataContextBase, IUsersDataContext
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersDataContext(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return _userManager.Users;
        }

        public Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<string> GetRoleForUserAsync(ApplicationUser user)
        {
            return (await _userManager.GetRolesAsync(user)).Single();
        }
    }
}
