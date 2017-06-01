using System;
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

        public async Task<ApplicationRole> GetRoleForUserAsync(ApplicationUser user)
        {
            var userRoleName = (await _userManager.GetRolesAsync(user)).Single();
            
            return await Context.Roles.SingleAsync(r => r.Name == userRoleName);
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update user '{user.UserName}' ({user.Id}): " +
                                    $"{string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return await GetUserByIdAsync(user.Id);
        }
    }
}
