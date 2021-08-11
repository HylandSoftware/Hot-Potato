using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HotPotato.AspNetCore.Host
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (Banner.Display(args))
            {
                Banner.Show();
            }
            var host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<Startup>()
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
