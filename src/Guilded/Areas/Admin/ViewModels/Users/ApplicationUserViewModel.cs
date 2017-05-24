using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public string Role { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? EnabledAfter { get; set; }

        public ApplicationUserViewModel()
        {
            Id = null;
            UserName = null;
            Email = null;
            Role = null;
        }

        public ApplicationUserViewModel(ApplicationUser user, string userRole)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = userRole;
            IsEnabled = user.IsEnabled;
            EnabledAfter = user.EnabledAfter;
        }
    }
}
