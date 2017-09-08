using Guilded.Data.Forums;
using Guilded.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Forums.ViewModels
{
    public class CreateThreadViewModel : IMarkdownContent
    {
        public const string TitleRegexPattern = @"^([\d+\s]*)\D+[\w\s.!@#$%^&*()_-]*";
        public const string TitleRegexErrorMessage = "Title cannot be exclusively numbers and spaces.";

        [HiddenInput]
        public int ForumId { get; set; }

        [HiddenInput]
        public string ForumSlug { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Thread.TitleRequiredErrorMessage)]
        [Display(Name = "Title")]
        [RegularExpression(TitleRegexPattern, ErrorMessage = TitleRegexErrorMessage)]
        [StringLength(Thread.TitleMaxLength, MinimumLength = Thread.TitleMinLength, ErrorMessage = Thread.TitleLengthErrorMessage)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = Thread.TitleRequiredErrorMessage)]
        [Display(Name = "Content")]
        [MinLength(Thread.ContentMinLength, ErrorMessage = Thread.ContentLengthErrorMessage)]
        public string Content { get; set; }

        public Thread ToThread(string authorId)
        {
            return new Thread
            {
                Content = Content,
                Title = Title,
                ForumId = ForumId,
                IsLocked = false,
                IsPinned = false,
                AuthorId = authorId
            };
        }
    }
}
