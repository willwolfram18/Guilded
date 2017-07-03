using Guilded.Data.Forums;
using Guilded.Data.Home;
using Guilded.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Guilded.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<GuildActivity> GuildActivity { get; set; }

        public DbSet<ForumSection> ForumSections { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Reply> Replies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Enforce unique role names.
            builder.Entity<ApplicationRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Define the many-to-one relationship for Threads to Forum.
            builder.Entity<Forum>()
                .HasMany(f => f.Threads)
                .WithOne(t => t.Forum)
                .IsRequired();

            
            // Define the many-to-one relationship for Replies to Thread.
            builder.Entity<Thread>()
                .HasMany(t => t.Replies)
                .WithOne(r => r.Thread)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
