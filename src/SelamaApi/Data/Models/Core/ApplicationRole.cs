using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SelamaApi.Data.Models.Core
{
    public class ApplicationRole : IdentityRole
    {
        #region Properties
        #region Navigation Properties
        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }

        [NotMapped]
        public IEnumerable<ResourcePrivilege> Privileges
        {
            get
            {
                return RolePrivileges.Select(r => r.Privilege);
            }
        }
        #endregion
        #endregion
    }
}