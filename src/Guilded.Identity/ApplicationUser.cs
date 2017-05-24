using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Guilded.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsEnabled { get; set; }

        public DateTime? EnabledAfter { get; set; }
    }
}
