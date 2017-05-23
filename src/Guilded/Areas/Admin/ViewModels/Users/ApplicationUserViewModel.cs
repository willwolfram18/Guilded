using System.ComponentModel.DataAnnotations;
using Guilded.Identity;

namespace Guilded.Areas.Admin.ViewModels.Users
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public ApplicationUserViewModel()
        {
            Id = null;
            UserName = null;
            Email = null;
        }

        public ApplicationUserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}
