using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guilded.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsEnabled { get; set; }

        public DateTime? EnabledAfter { get; set; }

        [NotMapped]
        public bool IsTemporarilyDisabled => EnabledAfter.HasValue;

        [NotMapped]
        public bool IsTemporaryDisableOver => EnabledAfter.HasValue && EnabledAfter.Value > DateTime.Today;
    }
}
