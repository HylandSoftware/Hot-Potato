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

        private const string ApiLocation = "http://localhost:5000";
        private const string SpecLocation = "\\test\\RawPotatoSpec.yaml";

        public HostFixture()
        {
            host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
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
                .UseSetting("RemoteEndpoint", ApiLocation)
                .UseSetting("SpecLocation", SpecLocation)
                .UseSetting("ForwardProxy:Enabled", "false")
                .UseSetting("ForwardProxy:ProxyAddress", "http://localhost:8888")
                .UseSetting("ForwardProxy:BypassOnLocal", "false")
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
