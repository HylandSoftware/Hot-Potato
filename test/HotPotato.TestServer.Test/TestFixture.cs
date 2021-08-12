using HotPotato.AspNetCore.Middleware;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;

namespace HotPotato.TestServ.Test
{
	public class TestFixture<TStartup> : IDisposable where TStartup : class
	{
		public HotPotatoClient Client { get; }
		public List<Result> Results { get; }

		//TestServer won't actually listen on an address, but it needs a BaseAddress to be used by the HttpRequest constructors
		//It can be set to any address as long as it is a valid uri
		private const string ApiServerAddress = "http://localhost:5000";
		//Same goes for the address of the TestServer housing the middleware:
		//it doesn't bind to an address, but we're setting it to 3232 here to mimic the host, and to give a base address to send requests
		private const string HotPotatoAddress = "http://localhost:3232";

		private readonly TestServer apiServer;
		private readonly TestServer hotPotatoServer;

		public TestFixture()
		{
			var apiBuilder = new WebHostBuilder()
				.UseStartup<TStartup>();

			apiServer = new TestServer(apiBuilder);
			apiServer.BaseAddress = new Uri(ApiServerAddress);

			HotPotatoClient apiClient = new HotPotatoClient(apiServer.CreateClient());

			var hotPotatoBuilder = new WebHostBuilder()
				//Setting this here instead of in appsettings.json so it always matches the BaseAddress on TestServer
				.UseSetting("RemoteEndpoint", ApiServerAddress)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
						.AddJsonFile("appsettings.json", optional: true)
						.AddEnvironmentVariables()
						.AddUserSecrets<TestFixture<TStartup>>();
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
				.ConfigureServices(services =>
				{
					services.ConfigureMiddlewareServices(apiClient);
				})
				.Configure(builder =>
				{
					builder.UseMiddleware<HotPotatoMiddleware>();
				});

			hotPotatoServer = new TestServer(hotPotatoBuilder);
			hotPotatoServer.BaseAddress = new Uri(HotPotatoAddress);

			Results = hotPotatoServer.Host.Services.GetService<IResultCollector>().Results;
			Client = new HotPotatoClient(hotPotatoServer.CreateClient());
		}

		public void Dispose()
		{
			apiServer.Dispose();
			hotPotatoServer.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
