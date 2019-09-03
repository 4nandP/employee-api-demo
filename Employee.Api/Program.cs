using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Employee.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            await webHost.RunAsync();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                   .ReadFrom.Configuration(hostingContext.Configuration)
                )
                .Build();
    }
}
