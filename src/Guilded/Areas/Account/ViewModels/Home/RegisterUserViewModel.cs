using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Account.ViewModels.Home
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [FromForm(Name = "g-recaptcha-response")]
        public string Recaptcha { get; set; }
    }
}