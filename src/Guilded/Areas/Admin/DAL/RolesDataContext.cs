using Guilded.DAL.Abstract;
using Guilded.Data;
using Guilded.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guilded.Security.Claims;

namespace Guilded.Areas.Admin.DAL
{
    public class RolesDataContext : ReadWriteDataContextBase, IRolesDataContext
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolesDataContext(ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }

        public IQueryable<ApplicationRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public Task<ApplicationRole> GetRoleByIdAsync(string id)
        {
            return _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ApplicationRole> CreateRoleAsync(ApplicationRole roleToCreate, IEnumerable<RoleClaim> roleClaims)
        {
            var result = await _roleManager.CreateAsync(roleToCreate);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create role '{roleToCreate.Name}'");
            }

            var createdRole = await GetRoleByIdAsync(roleToCreate.Id);

            foreach (var roleClaim in roleClaims)
            {
                await _roleManager.AddClaimAsync(createdRole, new Claim(RoleClaimTypes.Permission, roleClaim.ClaimValue));
            }

            return createdRole;
        }

        public async Task<ApplicationRole> UpdateRoleAsync(ApplicationRole roleToUpdate, IEnumerable<RoleClaim> roleClaims)
        {
            var result = await _roleManager.UpdateAsync(roleToUpdate);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update role '{roleToUpdate.Name}': " +
                                    $"{string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            var currentClaims = GetClaimsForRole(roleToUpdate).ToList();
            var setOfClaimsToRemove = new HashSet<Claim>(
                currentClaims.Where(c => roleClaims.All(rc => rc.ClaimValue != c.Value))
            );
            var setOfClaimsToAdd = new HashSet<Claim>(
                roleClaims.Where(rc => currentClaims.All(c => c.Value != rc.ClaimValue))
                    .Select(rc => new Claim(RoleClaimTypes.Permission, rc.ClaimValue))
            );

            foreach (var claim in setOfClaimsToRemove)
            {
                await _roleManager.RemoveClaimAsync(roleToUpdate, claim);
            }
            foreach (var claim in setOfClaimsToAdd)
            {
                await _roleManager.AddClaimAsync(roleToUpdate, claim);
            }

            return await GetRoleByIdAsync(roleToUpdate.Id);
        }

        public Task<IdentityResult> DeleteRoleAsync(ApplicationRole roleToDelete)
        {
            return _roleManager.DeleteAsync(roleToDelete);
        }

        public IEnumerable<Claim> GetClaimsForRole(ApplicationRole role)
        {
            IEnumerable<Claim> claims = new List<Claim>();

            Task.Run(async () =>
            {
                claims = await _roleManager.GetClaimsAsync(role);
            }).Wait();

            return claims;
        }
    }
}