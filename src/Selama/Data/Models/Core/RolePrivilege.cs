using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selama.Data.Models.Core
{
    [Table("AspNetRolePrivileges")]
    public class RolePrivilege
    {
        #region Properties
        #region Database columns
        [Required]
        [ForeignKey("Role")]
        public string RoleId { get; set; }

        [Required]
        [ForeignKey("Privilege")]
        public int PrivilegeId { get; set; }
        #endregion

        #region Navigation properties
        public virtual ApplicationRole Role { get; set; }
        public virtual ResourcePrivilege Privilege { get; set; }
        #endregion
        #endregion
    }
}