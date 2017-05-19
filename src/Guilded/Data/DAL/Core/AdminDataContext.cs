using Guilded.Areas.Admin.ViewModels.Roles;
using Guilded.Data.DAL.Abstract;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationRole = Guilded.Identity.ApplicationRole;

namespace Guilded.Data.DAL.Core
{
    public class AdminDataContext : ReadWriteDataContextBase, IAdminDataContext
    {
        #region Properties
        #region Private Properties
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionsRepository _permissions;
        #endregion
        #endregion

        public AdminDataContext(ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager,
            IPermissionsRepository permissions) : base(context)
        {
            _roleManager = roleManager;
            _permissions = permissions;
        }

        public IQueryable<ApplicationRole> GetRoles()
        {
            return _roleManager.Roles;
        }

        public ApplicationRole GetRoleById(string id)
        {
            return _roleManager.Roles.FirstOrDefault(r => r.Id == id);
        }

        public async Task<ApplicationRole> CreateRoleAsync(ApplicationRole roleToCreate)
        {
            var result = await _roleManager.CreateAsync(roleToCreate);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create role '{roleToCreate.Name}'");
            }
            return GetRoleById(roleToCreate.Id);
        }

        public async Task<ApplicationRole> UpdateRoleAsync(ApplicationRole roleToUpdate)
        {
            var result = await _roleManager.UpdateAsync(roleToUpdate);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update role '{roleToUpdate.Name}': {result.Errors.First().Description}");
            }
            return GetRoleById(roleToUpdate.Id);
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return _permissions.Get();
        }

        public async Task<IdentityResult> DeleteRole(ApplicationRole roleToDelete)
        {
            return await _roleManager.DeleteAsync(roleToDelete);
        }
    }
}