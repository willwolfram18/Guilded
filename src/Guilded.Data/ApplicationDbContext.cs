using Guilded.Data.Identity;
using Guilded.Data.Models.Forums;
using Guilded.Data.Models.Home;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guilded.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<GuildActivity> GuildActivity { get; set; }

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
            builder.Entity<ApplicationRole>()
                .HasIndex(r => r.Name)
                .IsUnique();
            base.OnModelCreating(builder);
        }
    }
}
