using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HotPotato.AspNetCore.Host;
using System;

namespace HotPotato.E2E.Test
{
	/// <summary>
	/// To avoid ambiguity, the purpose of these E2E tests is to run through the full http aspect of the system,
	/// while truncating the OpenApi aspect by setting up a mock server with a fake endpoint not in the Raw Potato spec.
	/// </summary>
	public class HostFixture : IDisposable
	{
		public IWebHost host { get; }
		public bool specTokenExists { get; }

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

			IConfiguration configuration = host.Services.GetService<IConfiguration>();
			if (!string.IsNullOrWhiteSpace(configuration["SpecToken"]))
			{
				specTokenExists = true;
			}

			host.Start();
		}

		public void Dispose()
		{
			host.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
