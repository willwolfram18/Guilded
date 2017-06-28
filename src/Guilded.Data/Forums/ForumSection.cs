using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guilded.Data.Forums
{
    /// <summary>
    /// Serves as a way to group different <see cref="Forum"/>s together
    /// </summary>
    [Table("ForumSections")]
    public class ForumSection
    {
        public const int TitleMaxLength = 35;
        public const int TitleMinLength = 4;
        public const string TitleRequiredErrorMessage = "The {0} cannot be blank";
        public const string TitleLengthErrorMessage = "The {0} must be between {2} and {1} characters in length";

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = TitleRequiredErrorMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthErrorMessage)]
        public string Title { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ConcurrencyCheck]
        public Guid Version { get; set; }

        public virtual ICollection<Forum> Forums { get; set; }
    }
}