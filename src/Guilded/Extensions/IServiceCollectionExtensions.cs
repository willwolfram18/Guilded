using Guilded.Areas.Admin.DAL;
using Guilded.Areas.Forums.DAL;
using Guilded.Common;
using Guilded.DAL;
using Guilded.DAL.Home;
using Guilded.Data;
using Guilded.Data.Forums;
using Guilded.Data.Home;
using Guilded.Data.Identity;
using Guilded.Security.Authorization;
using Guilded.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Guilded.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRequirementHandlers(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, RoleClaimAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, EnabledUserHandler>();
        }

        public static void AddGuildedIdentity(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Globals.OSX)
                {
                    options.UseSqlite(
                        databaseConnectionString,
                        opts => opts.MigrationsAssembly("Guilded.Migrations.Sqlite")
                    );
                }
                else
                {
                    options.UseSqlServer(
                        databaseConnectionString,
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

        public static void AddGuildedServices(this IServiceCollection services)
        {
            services.AddScoped<IReadOnlyRepository<GuildActivity>, GuildActivityRepo>();

            services.AddScoped<IGuildActivityReadOnlyDataContext, GuildActivityReadOnlyDataContext>();

            services.AddScoped<IRolesDataContext, RolesDataContext>();
            services.AddScoped<IUsersDataContext, UsersDataContext>();

            services.AddScoped<IForumsDataContext, ForumsDataContext>();

            services.AddScoped<IEmailSender, AuthMessageSender>();
            services.AddScoped<ISmsSender, AuthMessageSender>();

            services.AddScoped<IUserRoleClaimsForRequest, UserRoleClaimsForRequest>();
        }
    }
}
