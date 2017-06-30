using BattleNetApi.Apis.Interfaces;
using Guilded.Areas.Admin.DAL;
using Guilded.Common;
using Guilded.DAL;
using Guilded.DAL.Home;
using Guilded.Data;
using Guilded.Data.Home;
using Guilded.Data.Identity;
using Guilded.Security.Authorization;
using Guilded.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guilded.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRequirementHandlers(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, RoleClaimAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, EnabledUserHandler>();
        }

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
            services.AddTransient<IRolesDataContext, RolesDataContext>();
            services.AddTransient<IUsersDataContext, UsersDataContext>();
        }
    }
}
