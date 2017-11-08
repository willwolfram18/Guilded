using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Guilded.Data;
using Guilded.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guilded
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            var serviceScope = webHost.Services.GetService<IServiceScopeFactory>();
            using (var serviceResolver = serviceScope.CreateScope())
            {
                using (var db = serviceResolver.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    if (!db.AllMigrationsAreApplied())
                    {
                        db.Database.Migrate();
                    }
                    db.EnsureRequiredDataIsPresent();
                }
            }
            

            webHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
    }
}
