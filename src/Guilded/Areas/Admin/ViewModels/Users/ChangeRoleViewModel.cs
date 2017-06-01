using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Guilded.Areas.Admin.ViewModels.Users
{
    public class ChangeRoleViewModel
    {
        [Required]
        [HiddenInput]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Current Role")]
        [HiddenInput]
        public string OldRoleId { get; set; }

        [Required]
        [Display(Name = "New Role")]
        public string NewRoleId { get; set; }
    }
}
