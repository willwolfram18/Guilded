using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Guilded.Common;
using Guilded.Data;
using Guilded.Data.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Data.DAL;
using Guilded.Data.DAL.Home;
using Guilded.Data.Models.Home;
using BattleNetApi.Apis.Interfaces;
using Guilded.Options;
using Microsoft.IdentityModel.Tokens;
using Guilded.Services;
using Guilded.Data.DAL.Core;

namespace Guilded.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddSelama(this IServiceCollection services, IConfigurationRoot Configuration, SymmetricSecurityKey signingKey)
        {
            services.AddSelamaDb(Configuration);
            services.AddSelamaDAL(Configuration);
            services.AddSelamaOptions(Configuration, signingKey);

            services.AddSingleton<IBattleNetApi>(implementationInstance:
                new BattleNetApi.Apis.BattleNetApi(Configuration["OAuthProviders:BattleNetClientId"])
            );
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        private static void AddSelamaDb(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Globals.OSX)
                {
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection:OSX"));
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection:Windows"));
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

        private static void AddSelamaDAL(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddTransient<IReadOnlyRepository<GuildActivity>, GuildActivityRepo>();
            services.AddTransient<IGuildActivityReadOnlyDataContext, GuildActivityReadOnlyDataContext>();
            services.AddTransient<IPrivilegeReadWriteDataContext, PrivilegeReadWriteDataContext>();
        }

        private static void AddSelamaOptions(this IServiceCollection services, IConfigurationRoot Configuration, SymmetricSecurityKey signingKey)
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
