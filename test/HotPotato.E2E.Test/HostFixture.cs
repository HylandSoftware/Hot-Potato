using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HotPotato.AspNetCore.Host;
using System;

namespace HotPotato.E2E.Test
{
    public class HostFixture : IDisposable
    {
        public IWebHost host { get; }

        public HostFixture()
        {
            host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<HostFixture>();
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

            host.Start();
        }

        public void Dispose()
        {
            host.Dispose();
        }
    }
}
