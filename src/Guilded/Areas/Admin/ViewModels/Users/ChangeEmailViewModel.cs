using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Admin.ViewModels.Users
{
    public class ChangeEmailViewModel
    {
        [Required]
        [HiddenInput]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "New Email Address")]
        [EmailAddress]
        public string NewEmailAddress { get; set; }
    }
}
