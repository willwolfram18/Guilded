using Guilded.Data.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guilded.Data.Forums
{
    public class Reply
    {
        public const int ContentMinLength = 1;

        public const string ContentRequiredErrorMessage = ForumSection.TitleRequiredErrorMessage;
        public const string ContentLengthErrorMessage = "The {0}'s length must contain at least {1} characters.";

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ContentRequiredErrorMessage)]
        [MinLength(ContentMinLength, ErrorMessage = ContentLengthErrorMessage)]
        public string Content { get; set; }

        [Required]
        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Thread")]
        public int ThreadId { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Thread Thread { get; set; }
    }
}
