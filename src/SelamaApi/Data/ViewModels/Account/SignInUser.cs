using System.ComponentModel.DataAnnotations;

namespace SelamaApi.Data.ViewModels.Account
{
    public class SignInUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}