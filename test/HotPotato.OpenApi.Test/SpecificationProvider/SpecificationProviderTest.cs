using System;
using Xunit;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class OpenApiExceptionsTest
    {
        [Fact]
        public void SpecificationProvider_Constructor_ThrowsArgumentNullExceptionWithConfig()
        {
            Action subject = () => new SpecificationProvider(null);
            Assert.Throws<ArgumentNullException>(subject);
        }
    }
}
