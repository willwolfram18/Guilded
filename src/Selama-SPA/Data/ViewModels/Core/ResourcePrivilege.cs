using Newtonsoft.Json;
using System;
using DataModel = Selama.Data.Models.Core.ResourcePrivilege;

namespace Selama.Data.ViewModels.Core
{
    public class ResourcePrivilege
    {
        #region Properties
        #region Public Properties
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        #endregion
        #endregion

        public ResourcePrivilege()
        {
        }

        public ResourcePrivilege(DataModel privilege)
        {
            if (privilege == null)
            {
                throw new ArgumentNullException("privilege");
            }

            Id = privilege.Id;
            Name = privilege.Name;
        }
    }
}