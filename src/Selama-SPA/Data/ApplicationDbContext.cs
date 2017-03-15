using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Selama.Data.Models.Core;
using Selama.Data.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.Data.Models.Forums;

namespace Selama.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<GuildActivity> GuildActivity { get; set; }

        #region Resource privileges
        public DbSet<ResourcePrivilege> Privileges { get; set; }

        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        #endregion

        #region Forums
        public DbSet<ForumSection> ForumSections { get; set; }

        public DbSet<Forum> Forums { get; set; }
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RolePrivilege>()
                .HasKey("RoleId", "PrivilegeId");
            builder.Entity<ApplicationRole>()
                .HasMany(r => r.RolePrivileges)
                .WithOne(r => r.Role);
            builder.Entity<ResourcePrivilege>()
                .HasMany(r => r.RolePrivileges)
                .WithOne(r => r.Privilege);
            base.OnModelCreating(builder);
        }
    }
}
