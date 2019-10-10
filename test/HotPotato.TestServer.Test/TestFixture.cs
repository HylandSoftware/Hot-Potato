﻿using HotPotato.AspNetCore.Middleware;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;

namespace HotPotato.TestServ.Test
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        public Core.Http.Default.HttpClient Client { get; }
        public List<Result> Results { get; }

        //TestServer won't actually listen on an address, but it needs a BaseAddress to be used by the HttpRequest constructors
        //It can be set to any address as long as it is a valid uri
        private const string TestServerAddress = "http://localhost:5000";

        private readonly TestServer apiServer;
        private readonly TestServer hotPotatoServer;

        public TestFixture()
        {
            var apiBuilder = new WebHostBuilder()
                .UseStartup<TStartup>();

            apiServer = new TestServer(apiBuilder);
            apiServer.BaseAddress = new Uri(TestServerAddress);

            Core.Http.Default.HttpClient apiClient = new Core.Http.Default.HttpClient(apiServer.CreateClient());

            var hotPotatoBuilder = new WebHostBuilder()
                //Setting this here instead of in appsettings.json so it always matches the BaseAddress on TestServer
               .UseSetting("RemoteEndpoint", TestServerAddress)
               .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true);
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

            Results = hotPotatoServer.Host.Services.GetService<IResultCollector>().Results;
            Client = new Core.Http.Default.HttpClient(hotPotatoServer.CreateClient());
        }

        public void Dispose()
        {
            apiServer.Dispose();
            hotPotatoServer.Dispose();
        }
    }
}
