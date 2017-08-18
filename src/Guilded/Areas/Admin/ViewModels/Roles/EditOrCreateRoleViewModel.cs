using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class EditOrCreateRoleViewModel
    {
        [Required]
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "A role name must contain at least {1} characers")]
        public string Name { get; set; }

        public List<string> Permissions { get; set; }

        public List<RoleClaim> PermissionsAsRoleClaims
        {
            get
            {
                var claims = new List<RoleClaim>();

                foreach (var permission in Permissions)
                {
                    try
                    {
                        claims.Add(RoleClaimTypes.LookUpGuildedRoleClaim(permission));
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                }

                return claims;
            }
        }

        public SelectList AvailablePermissions { get; set; }

        public EditOrCreateRoleViewModel()
        {
            Permissions = new List<string>();
            AvailablePermissions = new SelectList(
                RoleClaimTypes.RoleClaims.OrderBy(c => c.ClaimType),
                "ClaimType",
                "ClaimType"
            );
        }

        public EditOrCreateRoleViewModel(ApplicationRole role) : this()
        {
            Id = role.Id;
            Name = role.Name;
            Permissions.AddRange(role.Claims.Select(c => c.ClaimType));
        }

        public ApplicationRole ToApplicationRole()
        {
            var role = new ApplicationRole
            {
                Id = Id,
                Name = Name,
            };

            foreach (var permission in PermissionsAsRoleClaims)
            {
                role.Claims.Add(new IdentityRoleClaim<string>
                {
                    ClaimType = permission.ClaimType,
                    ClaimValue = "True",
                    RoleId = role.Id
                });
            }

            return role;
        }
    }
}
