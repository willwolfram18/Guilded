using AspNet.Security.OAuth.BattleNet;
using Guilded.Security.Authorization;
using Guilded.Security.Claims;
using Guilded.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    builder.AddUserSecrets();
            //}
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();
            services.AddGuilded(Configuration);
            services.AddMvc().AddRazorOptions(razorOpts =>
            {
                razorOpts.ViewLocationExpanders.Add(new PartialsFolderViewLocationExpander());
            });

            services.AddAuthorization(opts =>
            {
                foreach (var roleClaim in RoleClaimTypes.RoleClaims)
                {
                    opts.AddPolicy(roleClaim.ClaimType, policy => policy.Requirements.Add(new RoleClaimAuthorizationRequirement(roleClaim)));
                }
            });
            
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<IdentityOptions>(opts =>
            {
                opts.Cookies.ApplicationCookie.LoginPath = new PathString("/account/sign-in");
                opts.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/access-denied");
            });

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IAuthorizationHandler, RoleClaimAuthorizationHandler>();
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
            app.UseIdentity();

            app.UseTwitterAuthentication(new TwitterOptions
            {
                ConsumerKey = "test",
                ConsumerSecret = "test"
            });
            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = "test",
                ClientSecret = "test",
            });
            app.UseFacebookAuthentication(new FacebookOptions
            {
                ClientId = "test",
                ClientSecret = "test"
            });
            app.UseBattleNetAuthentication(new BattleNetAuthenticationOptions
            {
                ClientId = "test",
                ClientSecret = "test",
                DisplayName = "Battle.net",
                Region = BattleNetAuthenticationRegion.America
            });


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
    }
}
