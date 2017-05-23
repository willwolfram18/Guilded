using Guilded.Identity;

namespace Guilded.Areas.Admin.ViewModels.Users
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ApplicationUserViewModel()
        {
            Id = null;
            Username = null;
            Email = null;
        }

        public ApplicationUserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            Username = user.UserName;
            Email = user.Email;
        }
    }
}
