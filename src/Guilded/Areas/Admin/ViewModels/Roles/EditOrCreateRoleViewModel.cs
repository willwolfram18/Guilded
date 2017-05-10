using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Identity;
using Guilded.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [Required]
        [HiddenInput]
        public string ConcurrencyStamp { get; set; }

        public List<string> Permissions { get; set; }

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
            ConcurrencyStamp = role.ConcurrencyStamp;
            Permissions.AddRange(role.Claims.Select(c => c.ClaimType));
        }
    }
}
