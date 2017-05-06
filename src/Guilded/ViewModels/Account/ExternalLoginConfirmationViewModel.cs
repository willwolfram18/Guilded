using System.ComponentModel.DataAnnotations;

namespace Guilded.ViewModels.Account
{
    public class ExternalSignInConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
