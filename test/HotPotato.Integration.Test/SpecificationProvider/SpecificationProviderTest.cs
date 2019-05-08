
using static HotPotato.IntegrationTestMethods;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class SpecificationProviderTest
    {
        [Fact]
        public void ISpecificationProvider_ReturnsDocumentFromPath()
        {
            ServiceProvider provider = GetServiceProvider(SpecPath("specs/keyword/", "specification.yaml"));

            ISpecificationProvider subject = provider.GetService<ISpecificationProvider>();
            SwaggerDocument result = subject.GetSpecDocument();

            Assert.Equal(result.DocumentPath, SpecPath("specs/keyword/", "specification.yaml"));
        }
    }
}
