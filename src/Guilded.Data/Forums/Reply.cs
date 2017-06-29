using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Data.Identity;

namespace Guilded.Data.Forums
{
    public class Reply
    {
        public const int ContentMinLength = 25;

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
        public DateTime CreatedAt { get; set; }

        [Required]
        [ForeignKey("Thread")]
        public int ThreadId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Thread Thread { get; set; }
    }
}
