using Guilded.Data.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

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
                        // Skip permissions which are not valid
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

        public EditOrCreateRoleViewModel(ApplicationRole role, IEnumerable<Claim> claims) : this()
        {
            Id = role.Id;
            Name = role.Name;
            Permissions.AddRange(claims.Select(c => c.Type));
        }

        public ApplicationRole ToApplicationRole()
        {
            return new ApplicationRole
            {
                Id = Id,
                Name = Name,
            };
        }
    }
}
