using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Guilded.Data.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; } = new List<IdentityRoleClaim<string>>();
    }
}