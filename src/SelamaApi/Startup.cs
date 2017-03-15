using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SelamaApi.Data;
using SelamaApi.Common;
using Microsoft.EntityFrameworkCore;
using SelamaApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SelamaApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SelamaApi
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

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        private const string CORS_POLICY_NAME = "SelamaApi-AngularApp";
        private const string SECRET_KEY = "SampleNotSoSecretKey";
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY));

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();
            services.AddSelama(Configuration, _signingKey);
            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter("SelamaApi Ashalanore"));
            });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("SelamaApi Ashalanore", policy => policy.RequireClaim(Globals.JWT_CLAIM_TYPE, Globals.JWT_CLAIM_VALUE));
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors(opts =>
            {
                opts.AddPolicy(
                    CORS_POLICY_NAME,
                    // TODO: Use config for "with origins"
                    builder => builder.WithOrigins("http://localhost:8000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });
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
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(CORS_POLICY_NAME);
            app.UseIdentity();
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = CreateTokenValidationParameters(),
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        private TokenValidationParameters CreateTokenValidationParameters()
        {
            var jwtOptions = Configuration.GetSection("JwtOptions");
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions["Issuer"],

                ValidateAudience = true,
                ValidAudience = (Globals.OSX ? jwtOptions["Audience:OSX"] : jwtOptions["Audience:Windows"]),

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero,
            };
        }
    }
}
