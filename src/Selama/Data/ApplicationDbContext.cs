using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Selama.Models;
using Selama.Models.Home;

namespace Selama.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Properties
        #region Public properties
        public DbSet<GuildActivity> GuildActivities { get; set; }
        #endregion
        #endregion

        #region Constructors
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
