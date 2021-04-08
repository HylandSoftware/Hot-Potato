
using static HotPotato.IntegrationTestMethods;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using System;
using Xunit;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class SpecificationProviderTest
    {
        [Fact]
        public void ISpecificationProvider_GetSpecDocument_ReturnsDocumentFromPath()
        {
            ServiceProvider provider = GetServiceProvider(SpecPath("specs/keyword/", "specification.yaml"));

            ISpecificationProvider subject = provider.GetService<ISpecificationProvider>();
            OpenApiDocument result = subject.GetSpecDocument();

            Assert.Equal(result.DocumentPath, SpecPath("specs/keyword/", "specification.yaml"));
        }

        [Fact]
        public void ISpecificationProvider_GetSpecDocument_ThrowsInvalidOperationWithInvalidLocation()
        {
            ServiceProvider provider = GetServiceProvider(string.Empty);

            ISpecificationProvider subject = provider.GetService<ISpecificationProvider>();

            Action result = () => subject.GetSpecDocument();
            Assert.Throws<InvalidOperationException>(result);
        }
    }
}
