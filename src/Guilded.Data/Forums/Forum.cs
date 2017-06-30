using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guilded.Data.Forums
{
    [Table("Forums")]
    public class Forum
    {
        public const int MinTitleLength = 4;
        public const int MaxTitleLength = 35;
        public const int MaxSubtitleLength = 85;
        public const int MaxSlugLength = MaxTitleLength * 2;

        public const string TitleRequiredErrorMessage = ForumSection.TitleRequiredErrorMessage;
        public const string TitleLengthErrorMessage = ForumSection.TitleLengthErrorMessage;
        public const string SubtitleLengthErrorMessage = "The {0}'s length cannot exceed {1} characters";

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = TitleRequiredErrorMessage)]
        [StringLength(MaxTitleLength, MinimumLength = MinTitleLength, ErrorMessage = TitleLengthErrorMessage)]
        public string Title { get; set; }

        [StringLength(MaxSubtitleLength, ErrorMessage = SubtitleLengthErrorMessage)]
        public string Subtitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = TitleRequiredErrorMessage)]
        [StringLength(MaxTitleLength, MinimumLength = MinTitleLength, ErrorMessage = TitleLengthErrorMessage)]
        public string Slug { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("ForumSection")]
        public int ForumSectionId { get; set; }

        [ConcurrencyCheck]
        public Guid Version { get; set; } = Guid.NewGuid();

        public virtual ForumSection ForumSection { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
    }
}