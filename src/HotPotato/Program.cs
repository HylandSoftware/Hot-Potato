using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotPotato
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddDebug();
                    }
                })
                .UseKestrel((options) =>
                {
                    options.AddServerHeader = false;
                })
                .UseUrls("http://0.0.0.0:3232")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
