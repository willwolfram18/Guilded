using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selama_SPA.Data.Models.Forums
{
    [Table("ForumSections")]
    public class ForumSection
    {
        #region Properties
        #region Database properties
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The title cannot be blank")]
        [StringLength(35, MinimumLength = 4, ErrorMessage = "The title must be between {0} and {1} characters in length")]
        public string Title { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        #endregion

        #region Navigation properties
        public virtual ICollection<Forum> Forums { get; set; }
        #endregion
        #endregion
    }
}