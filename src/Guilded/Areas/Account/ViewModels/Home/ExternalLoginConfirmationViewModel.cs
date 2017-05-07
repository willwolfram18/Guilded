using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Account.ViewModels.Home
{
    public class ExternalSignInConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
