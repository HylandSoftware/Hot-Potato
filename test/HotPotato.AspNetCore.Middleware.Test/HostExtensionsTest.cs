using HotPotato.Core.Cookies;
using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Processor;
using HotPotato.Core.Proxy;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace HotPotato.AspNetCore.Middleware
{
    public class HostExtensionsTest
    {
        private const string SpecLocation = "https://bitbucket.hylandqa.net/projects/AUTOTEST/repos/hot-potato/raw/test/RawPotatoSpec.yaml";
        private const string ApiServerAddress = "http://localhost:5000";

        [Fact]
        public void ConfigureMiddlewareServices_SetsAllExpectedServices()
        {
            HttpClient client = new HttpClient(new System.Net.Http.HttpClient());

            IWebHost subject = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                })
                .ConfigureServices(services =>
                {
                    services.ConfigureMiddlewareServices(client);
                })
                .UseSetting("SpecLocation", SpecLocation)
                .UseSetting("RemoteEndpoint", ApiServerAddress)
                .Configure(app =>
                {
                    app.UseMiddleware<HotPotatoMiddleware>();
                })
                .Build();

            IServiceProvider result = subject.Services;

            //tried to make this into loop of type variables,
            //but the compiler didn't like using variables as Types
            Assert.NotNull(result.GetService<IProxy>());
            Assert.NotNull(result.GetService<IHttpClient>());
            Assert.Equal(client, result.GetService<IHttpClient>());
            Assert.NotNull(result.GetService<ISpecificationProvider>());
            Assert.NotNull(result.GetService<IResultCollector>());
            Assert.NotNull(result.GetService<IProcessor>());
            Assert.NotNull(result.GetService<ICookieJar>());
        }

        [Fact]
        public void ConfigureMiddlewareServices_Creates_HttpClient_Via_DI()
        {
            IWebHost subject = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                })
                .ConfigureServices(services =>
                {
                    services.ConfigureMiddlewareServices();
                })
                .UseSetting("SpecLocation", SpecLocation)
                .Configure(app =>
                {
                    app.UseMiddleware<HotPotatoMiddleware>();
                })
                .Build();

            IServiceProvider result = subject.Services;

            Assert.NotNull(result.GetService<IHttpClient>());
        }
    }
}