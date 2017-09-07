using Guilded.Data.Forums;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Forums.ViewModels
{
    public class CreateReplyViewModel : IMarkdownContent
    {
        [HiddenInput]
        public int ThreadId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Reply.ContentRequiredErrorMessage)]
        [Display(Name = "Content")]
        [MinLength(Reply.ContentMinLength, ErrorMessage = Reply.ContentLengthErrorMessage)]
        public string Content { get; set; }
    }
}
