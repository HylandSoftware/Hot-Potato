using System;
using Xunit;

namespace HotPotato.OpenApi
{
    public class OpenApiExceptionsTest
    {
        [Fact]
        public void SpecificationProvider_Constructor_ThrowsArgumentNullExceptionWithConfig()
        {
            Action subject = () => new SpecificationProvider.SpecificationProvider(null);
            Assert.Throws<ArgumentNullException>(subject);
        }
    }
}
