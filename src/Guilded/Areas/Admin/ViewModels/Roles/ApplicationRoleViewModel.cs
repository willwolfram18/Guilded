using Guilded.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class ApplicationRoleViewModel
    {
        #region Properties
        #region Public Properties
        [Required]
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "A role name must contain at least {1} characers")]
        public string Name { get; set; }

        [Required]
        [HiddenInput]
        public string ConcurrencyStamp { get; set; }

        public IList<Permission> Permissions { get; set; }
        #endregion
        #endregion

        public ApplicationRoleViewModel()
        {
            Id = null;
            Name = null;
            ConcurrencyStamp = Guid.NewGuid().ToString();
            Permissions = new List<Permission>();
        }

        public ApplicationRoleViewModel(ApplicationRole role) : this()
        {
            if (role == null)
            {
                return;
            }

            Id = role.Id;
            Name = role.Name;
            ConcurrencyStamp = role.ConcurrencyStamp;
            Permissions = Permissions.OrderBy(p => p.PermissionType)
                .ToList();
        }
    }
}