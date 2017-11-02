using System.Linq;
using System.Security.Claims;
using Guilded.Common;
using Guilded.Extensions;
using Guilded.Security.Authorization;
using Guilded.Security.Claims;
using Guilded.Services;
using Guilded.ViewLocationExpanders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Guilded
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var hostingPlatform = Globals.OSX ? "OSX" : "Windows";

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingPlatform}.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    builder.AddUserSecrets();
            //}
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();
            services.AddGuildedIdentity(SqlConnectionString());
            services.AddGuildedServices();

            services.AddMvc().AddRazorOptions(razorOpts =>
            {
                razorOpts.ViewLocationExpanders.Add(new PartialsFolderViewLocationExpander());
                razorOpts.ViewLocationExpanders.Add(new DisplayTemplateFolderViewLocationExpander());
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(AuthorizeEnabledUserAttribute.PolicyName, policy => policy.Requirements.Add(new EnabledUserRequirement()));

                foreach (var roleClaim in RoleClaimValues.RoleClaims)
                {
                    opts.AddPolicy(roleClaim.ClaimValue, policy => policy.Requirements.Add(new RoleClaimAuthorizationRequirement(roleClaim)));
                }
            });
            
            services.AddRouting(options => options.LowercaseUrls = true);
            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = new PathString("/account/sign-in");
                opts.AccessDeniedPath = new PathString("/access-denied");
            });
            services.AddAuthentication()
                .AddFacebook(opts =>
                {
                    opts.AppId = "xxx";
                    opts.AppSecret = "xxx";
                })
                .AddGoogle(opts =>
                {
                    opts.ClientId = "xxx";
                    opts.ClientSecret = "xxx";
                })
                .AddTwitter(opts =>
                {
                    opts.ConsumerKey = "xxx";
                    opts.ConsumerSecret = "xxx";
                });

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IMarkdownConverter, MarkdownConverter>();
            services.AddRequirementHandlers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            app.UseStaticFiles();
            app.UseAuthentication();

            //app.UseBattleNetAuthentication(new BattleNetAuthenticationOptions
            //{
            //    ClientId = "test",
            //    ClientSecret = "test",
            //    DisplayName = "Battle.net",
            //    Region = BattleNetAuthenticationRegion.America
            //});


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultArea",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        private string SqlConnectionString()
        {
            var sqlServer = Configuration.GetValue<string>("SQL_SERVER_HOST");
            var sqlUser = Configuration.GetValue<string>("SQL_USER");
            var sqlUserPassword = Configuration.GetValue<string>("SQL_USER_PASSWORD");

            return $"Server={sqlServer};Database=Guilded;User={sqlUser};Password={sqlUserPassword};" +
                   "MultipleActiveResultSets=True;";
        }
    }
}
