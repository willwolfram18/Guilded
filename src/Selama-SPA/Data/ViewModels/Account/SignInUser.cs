using System.ComponentModel.DataAnnotations;

namespace Selama_SPA.Data.ViewModels.Account
{
    public class SignInUser
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}