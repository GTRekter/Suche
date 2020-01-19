using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Suche.Models.Context;

namespace Suche
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // get the IWebHost which will host this application.
            var host = CreateWebHostBuilder(args).Build();

            // find the service layer within our scope.
            using (var scope = host.Services.CreateScope())
            {
                // get the instance of ApplicationDBContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                // call the Initialize to create sample data
                ApplicationDbContext.Initialize(services);
            }

            // continue to run the application
            host.Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
