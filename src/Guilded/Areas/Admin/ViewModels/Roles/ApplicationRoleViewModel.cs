using Guilded.Data.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<Permission> Permissions { get; set; }

        public ApplicationRoleViewModel()
        {
            Id = null;
            Name = null;
            Permissions = new List<Permission>();
        }

        public ApplicationRoleViewModel(ApplicationRole role, IEnumerable<Claim> claims) : this()
        {
            if (role == null)
            {
                return;
            }

            Id = role.Id;
            Name = role.Name;
            Permissions = claims.OrderBy(c => c.Type)
                .Select(c => new Permission(c))
                .ToList();
        }
    }
}