using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Selama_SPA.Data.ViewModels.Core
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

        [JsonProperty("privileges")]
        public IList<ResourcePrivilege> Privileges { get; set; }
        #endregion
        #endregion
        
        public ApplicationRole()
        {
            Id = null;
            Name = null;
            Privileges = new List<ResourcePrivilege>();
        }
    }
}