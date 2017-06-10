using Guilded.Data.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Admin.ViewModels.Users
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string RoleId { get; set; }

        public string Role { get; set; }

        public bool IsEnabled { get; set; }

        public bool EmailConfirmed { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EnabledAfter { get; set; }

        public ApplicationUserViewModel()
        {
            Id = null;
            UserName = null;
            Email = null;
            Role = null;
        }

        public ApplicationUserViewModel(ApplicationUser user, ApplicationRole userRole)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            RoleId = userRole.Id;
            Role = userRole.Name;
            IsEnabled = user.IsEnabled;
            EnabledAfter = user.EnabledAfter;
            EmailConfirmed = user.EmailConfirmed;
        }
    }
}
