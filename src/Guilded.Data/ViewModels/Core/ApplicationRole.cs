using Guilded.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataModel = Guilded.Data.Models.Core.ApplicationRole;

namespace Guilded.Data.ViewModels.Core
{
    public class ApplicationRole
    {
        #region Properties
        #region Public Properties
        [Required]
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("name")]
        [MinLength(5, ErrorMessage = "A role name must contain at least {0} characers")]
        public string Name { get; set; }

        [JsonProperty("permissions")]
        public IList<Permission> Permissions { get; set; }
        #endregion
        #endregion

        public ApplicationRole()
        {
            Id = null;
            Name = null;
            Permissions = new List<Permission>();
        }

        public ApplicationRole(DataModel role) : this()
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            Id = role.Id;
            Name = role.Name;
            Permissions = role.Claims.ToListOfDifferentType(p => new Permission(p))
                .OrderBy(p => p.PermissionType)
                .ToList();
        }
    }
}