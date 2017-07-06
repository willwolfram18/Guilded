using System.ComponentModel;
using Guilded.Data.Forums;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Forums.ViewModels
{
    public class CreateThreadViewModel : IMarkdownContent
    {
        [HiddenInput]
        public int ForumId { get; set; }

        [HiddenInput]
        public string ForumSlug { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Thread.TitleRequiredErrorMessage)]
        [Display(Name = "Title")]
        [StringLength(Thread.TitleMaxLength, MinimumLength = Thread.TitleMinLength, ErrorMessage = Thread.TitleLengthErrorMessage)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Thread.TitleRequiredErrorMessage)]
        [Display(Name = "Content")]
        [MinLength(Thread.ContentMinLength, ErrorMessage = Thread.ContentLengthErrorMessage)]
        public string Content { get; set; }
    }
}
