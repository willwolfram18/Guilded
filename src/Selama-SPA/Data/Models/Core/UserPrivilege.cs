using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selama_SPA.Data.Models.Core
{
    [Table("AspNetUserPrivileges")]
    public class UserPrivilege
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }
    }
}
