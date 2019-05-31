
using HotPotato.Core.Proxy;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace HotPotato.AspNetCore.Middleware
{
    public class MiddlewareExceptionsTest
    {
        [Fact]
        public void HotPotatoMiddleware_Constructor_ThrowsArgumentNullExceptionWithProxy()
        {
            Action subject = () => new HotPotatoMiddleware(null, null, null, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HotPotatoMiddleware_Constructor_ThrowsArgumentNullExceptionWithConfig()
        {
            Action subject = () => new HotPotatoMiddleware(null, Mock.Of<IProxy>(), null, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HotPotatoMiddleware_Constructor_ThrowsArgumentNullExceptionWithLogger()
        {
            Action subject = () => new HotPotatoMiddleware(null, Mock.Of<IProxy>(), Mock.Of<IConfiguration>(), null);
            Assert.Throws<ArgumentNullException>(subject);
        }
    }
}
