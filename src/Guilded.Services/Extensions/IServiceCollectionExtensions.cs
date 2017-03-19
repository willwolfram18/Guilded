using BattleNetApi.Apis.Interfaces;
using Guilded.Common;
using Guilded.Common.Options;
using Guilded.Data;
using Guilded.Data.DAL;
using Guilded.Data.DAL.Core;
using Guilded.Data.DAL.Home;
using Guilded.Data.Models.Core;
using Guilded.Data.Models.Home;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Guilded.Services.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddGuilded(this IServiceCollection services, IConfigurationRoot Configuration, SymmetricSecurityKey signingKey)
        {
            services.AddGuildedDb(Configuration);
            services.AddGuildedDAL(Configuration);
            services.AddGuildedOptions(Configuration, signingKey);

            services.AddSingleton<IBattleNetApi>(implementationInstance:
                new BattleNetApi.Apis.BattleNetApi(Configuration["OAuthProviders:BattleNetClientId"])
            );
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        private static void AddGuildedDb(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Globals.OSX)
                {
                    options.UseSqlite(
                        Configuration.GetConnectionString("DefaultConnection:OSX"),
                        opts => opts.MigrationsAssembly("Guilded.Migrations.Sqlite")
                    );
                }
                else
                {
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection:Windows"),
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

        private static void AddGuildedDAL(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddTransient<IReadOnlyRepository<GuildActivity>, GuildActivityRepo>();
            services.AddTransient<IGuildActivityReadOnlyDataContext, GuildActivityReadOnlyDataContext>();
            services.AddTransient<IPrivilegeReadWriteDataContext, PrivilegeReadWriteDataContext>();
        }

        private static void AddGuildedOptions(this IServiceCollection services, IConfigurationRoot Configuration, SymmetricSecurityKey signingKey)
        {
            services.Configure<JwtOptions>(options =>
            {
                var optionSettings = Configuration.GetSection("JwtOptions");
                options.Issuer = optionSettings["Issuer"];
                if (Globals.OSX)
                {
                    options.Audience = optionSettings["Audience:OSX"];
                }
                else
                {
                    options.Audience = optionSettings["Audience:Windows"];
                }
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }
    }
}
