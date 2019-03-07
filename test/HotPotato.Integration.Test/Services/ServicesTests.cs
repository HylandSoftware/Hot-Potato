using HotPotato.AspNetCore.Host;
using static HotPotato.IntegrationTestMethods;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Services
{
    public class ServicesTests
    {
        [Fact]
        public void IDocumentProvider_ReturnsDocumentFromPath()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
                ;
            config["SpecLocation"] = SpecPath("specs/keyword/", "specification.yaml");

            Mock<ILogger<Startup>> logMock = new Mock<ILogger<Startup>>();
            Startup startUp = new Startup(config, logMock.Object);

            IServiceCollection services = new ServiceCollection();
            startUp.ConfigureServices(services);
            services.AddSingleton<IConfiguration>(config);
            ServiceProvider provider = services.BuildServiceProvider();

            IDocumentProvider subject = provider.GetService<IDocumentProvider>();
            SwaggerDocument result = subject.GetSpecDocument();

            Assert.Equal(result.DocumentPath, config["SpecLocation"]);
        }
    }
}
