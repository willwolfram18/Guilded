using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelamaApi.Data.Models.Forums
{
    [Table("ForumSections")]
    public class ForumSection
    {
        #region Properties
        #region Database properties
        public const int TITLE_MAX_LENGTH = 35;
        public const int TITLE_MIN_LENGTH = 4;
        public const string REQUIRED_ERROR_MSG = "The title cannot be blank";
        public const string LENGTH_ERROR_MSG = "The title must be between {0} and {1} characters in length";

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = REQUIRED_ERROR_MSG)]
        [StringLength(TITLE_MAX_LENGTH, MinimumLength = TITLE_MIN_LENGTH, ErrorMessage = LENGTH_ERROR_MSG)]
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