using Guilded.DAL.Abstract;
using Guilded.Data;
using Guilded.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.DAL
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

        public Task<ApplicationUser> GetUserFromClaimsPrincipalAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
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

        public async Task<ApplicationUser> ChangeUserRoleAsync(ApplicationUser user, ApplicationRole role)
        {
            var currentRole = await GetRoleForUserAsync(user);

            var result = await _userManager.RemoveFromRoleAsync(user, currentRole.Name);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to remove user '{user.UserName}' from current role '{currentRole.Name}'");
            }

            result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to add user '{user.UserName}' to role '{role.Name}'");
            }

            return await GetUserByIdAsync(user.Id);
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
    }
}
