using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Account.ViewModels
{
    public class SignInUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }
}