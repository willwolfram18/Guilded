using BattleNetApi.Apis.Interfaces;
using Guilded.Common;
using Guilded.Data;
using Guilded.Data.DAL;
using Guilded.Data.DAL.Core;
using Guilded.Data.DAL.Home;
using Guilded.Data.Models.Home;
using Guilded.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guilded.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddGuilded(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddGuildedDb(configuration);
            services.AddGuildedDAL();

            services.AddSingleton<IBattleNetApi>(
                new BattleNetApi.Apis.BattleNetApi(configuration["OAuthProviders:BattleNetClientId"])
            );
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        private static void AddGuildedDb(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Globals.OSX)
                {
                    options.UseSqlite(
                        configuration.GetConnectionString("DefaultConnection:OSX"),
                        opts => opts.MigrationsAssembly("Guilded.Migrations.Sqlite")
                    );
                }
                else
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection:Windows"),
                        opts => opts.MigrationsAssembly("Guilded.Migrations.SqlServer")
                    );
                }
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddDefaultTokenProviders();
        }

        private static void AddGuildedDAL(this IServiceCollection services)
        {
            services.AddTransient<IReadOnlyRepository<GuildActivity>, GuildActivityRepo>();
            services.AddTransient<IGuildActivityReadOnlyDataContext, GuildActivityReadOnlyDataContext>();
            services.AddTransient<IPermissionsRepository, PermissionsRepository>();
            services.AddTransient<IAdminDataContext, AdminDataContext>();
        }
    }
}
