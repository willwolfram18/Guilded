using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Selama.Data;
using Selama.Models;
using Selama.Services;
using Microsoft.AspNetCore.Mvc;
using Selama.Common;
using AspNet.Security.OAuth.BattleNet;
using Selama.Data.DAL;
using Selama.Data.DAL.Home;
using BattleNetApi.Apis.Interfaces;
using BattleNetApi.Apis;
using Selama.Models.Home;

namespace Selama
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Globals.OS_X)
                {
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection_OSX"));
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                }
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddRouting(opts =>
            {
                opts.LowercaseUrls = true;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();

            // Add application services.
            RegisterDependencyInjections(services);
        }

        private static void RegisterDependencyInjections(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IEntityRepo<GuildNewsFeedItem>, GuildNewsFeedRepo>();
            services.AddSingleton<IBattleNetApi>(implementationInstance: 
                new BattleNetApi.Apis.BattleNetApi("")
            );
            services.AddTransient<IGuildNewsUnitOfWork, GuildNewsUnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
            app.UseIdentity();
            InitOAuthProviders(app);

            app.UseSession();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "areaDefault",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        private void InitOAuthProviders(IApplicationBuilder app)
        {
            var OAuthProviders = Configuration.GetSection("OAuthProviders");
            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = OAuthProviders.GetValue<string>("GoogleClientId"),
                ClientSecret = OAuthProviders.GetValue<string>("GoogleClientSecret"),
            });
            app.UseFacebookAuthentication(new FacebookOptions
            {
                ClientId = OAuthProviders.GetValue<string>("FacebookClientId"),
                ClientSecret = OAuthProviders.GetValue<string>("FacebookClientSecret"),
            });
            app.UseBattleNetAuthentication(new BattleNetAuthenticationOptions
            {
                ClientId = OAuthProviders.GetValue<string>("BattleNetClientId"),
                ClientSecret = OAuthProviders.GetValue<string>("BattleNetClientSecret"),
                Region = BattleNetAuthenticationRegion.America,
                DisplayName = "Battle.net",
            });
        }
    }
}
