using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class OpenApiExceptionsTest
    {
        [Fact]
        public void SpecificationProvider_Constructor_ThrowsArgumentNullExceptionWithConfig()
        {
            ILogger<SpecificationProvider> mockLogger = Mock.Of<ILogger<SpecificationProvider>>();
            Action subject = () => new SpecificationProvider(null, mockLogger);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void SpecificationProvider_Constructor_ThrowsArgumentNullExceptionWithLogger()
        {
            IConfiguration mockConfig = Mock.Of<IConfiguration>();
            Action subject = () => new SpecificationProvider(mockConfig, null);
            Assert.Throws<ArgumentNullException>(subject);
        }
    }
}
