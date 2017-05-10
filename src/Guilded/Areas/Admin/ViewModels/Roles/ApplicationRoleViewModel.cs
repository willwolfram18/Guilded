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
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<Permission> Permissions { get; set; }
        #endregion
        #endregion

        public ApplicationRoleViewModel()
        {
            Id = null;
            Name = null;
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
            Permissions = role.Claims.OrderBy(c => c.ClaimType)
                .Select(c => new Permission(c))
                .ToList();
        }
    }
}