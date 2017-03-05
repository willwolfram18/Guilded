using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Selama_SPA.Data.Models.Core;
using Selama_SPA.Data.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama_SPA.Data.Models.Forums;

namespace Selama_SPA.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<GuildActivity> GuildActivity { get; set; }

        public DbSet<UserPrivilege> Privileges { get; set; }

        #region Forums
        public DbSet<ForumSection> ForumSections { get; set; }

        public DbSet<Forum> Forums { get; set; }
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
