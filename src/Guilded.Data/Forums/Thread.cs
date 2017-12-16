using Guilded.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guilded.Data.Forums
{
    public class Thread
    {
        public const int TitleMaxLength = 50;
        public const int TitleMinLength = 4;
        public const int SlugMaxLength = TitleMaxLength * 2;
        public const int ContentMinLength = 25;

        public const string TitleRequiredErrorMessage = ForumSection.TitleRequiredErrorMessage;
        public const string TitleLengthErrorMessage = ForumSection.TitleLengthErrorMessage;
        public const string ContentLengthErrorMessage = "The {0}'s length must contain at least {1} characters.";

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(SlugMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthErrorMessage)]
        public string Slug { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = TitleRequiredErrorMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = TitleRequiredErrorMessage)]
        [MinLength(ContentMinLength, ErrorMessage = ContentLengthErrorMessage)]
        public string Content { get; set; }

        [Required]
        public bool IsPinned { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Forum")]
        public int ForumId { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Forum Forum { get; set; }
        
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
