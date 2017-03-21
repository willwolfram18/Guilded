using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Guilded.Data.Models.Core
{
    public class ApplicationRole : IdentityRole
    {
        #region Properties
        #region Database columns
        [ForeignKey("ParentRole")]
        public string ParentRoleId { get; set; }
        #endregion

        #region Navigation Properties
        public virtual ApplicationRole ParentRole { get; set; }

        [InverseProperty("ParentRole")]
        public virtual ICollection<ApplicationRole> ChildRoles { get; set; }
        #endregion
        #endregion
    }
}