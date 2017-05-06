using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Account.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
