using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelamaApi.Data.Models.Forums
{
    [Table("Forums")]
    public class Forum
    {
        #region Properties
        #region Database Properties
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The title cannot be blank")]
        [StringLength(35, MinimumLength = 4, ErrorMessage = "The title must be between {0} and {1} characters in length")]
        public string Title { get; set; }

        [StringLength(85, ErrorMessage = "The subtitle's length cannot exceed {0} characters")]
        public string SubTitle { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("ForumSection")]
        public int ForumSectionId { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        #endregion

        #region Navigation Properties
        public virtual ForumSection ForumSection { get; set; }
        #endregion
        #endregion
    }
}