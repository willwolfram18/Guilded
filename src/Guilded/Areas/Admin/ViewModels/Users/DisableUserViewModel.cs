using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Guilded.Areas.Admin.ViewModels.Users
{
    public class DisableUserViewModel
    {
        [Required]
        [HiddenInput]
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Enabled after")]
        public DateTime? EnableAfter { get; set; }
    }
}
