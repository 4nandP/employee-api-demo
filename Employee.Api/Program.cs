using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System.Threading.Tasks;

namespace Employee.Api
{
    /// <summary>
    /// The program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static async Task Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            await webHost.RunAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .Build();
    }
}
