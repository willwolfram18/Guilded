using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Selama.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Selama
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder host = SetupWebHostBuilder();

            host.Build().Run();
        }

        private static IWebHostBuilder SetupWebHostBuilder()
        {
            var host = new WebHostBuilder()
                .UseKestrel(opts =>
                {
                    if (Globals.OSX)
                    {
                        opts.UseHttps("testCert.pfx", "testPassword");
                    }
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
            if (Globals.OSX)
            {
                host.UseUrls("https://localhost:44358");
            }

            return host;
        }
    }
}
