using DataModel = Selama_SPA.Data.Models.Core.ResourcePrivilege;

namespace Selama_SPA.Data.ViewModels.Core
{
    public class ResourcePrivilege
    {
        public ResourcePrivilege()
        {
        }
        public ResourcePrivilege(DataModel privilege)
        {
            Id = privilege.Id;
            Name = privilege.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}