using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HotPotato.Middleware
{
    public class ProxyMiddlewareTest
    {
        private const string RemoteEndpointKey = "RemoteEndpoint";
        private const string AValidEndpoint = "http://foo";

        [Fact]
        public void Constructor_NullProxy_Throws()
        {
            Assert.Throws<ArgumentNullException>("proxy", () => new ProxyMiddleware(null, null, Mock.Of<IConfiguration>(), Mock.Of<ILogger<ProxyMiddleware>>()));
        }

        [Fact]
        public void Constructor_NullConfiguration_Throws()
        {
            Assert.Throws<ArgumentNullException>("configuration", () => new ProxyMiddleware(null, Mock.Of<IProxy>(), null, Mock.Of<ILogger<ProxyMiddleware>>()));
        }

        [Fact]
        public void Constructor_NullLogger_Throws()
        {
            Assert.Throws<ArgumentNullException>("log", () => new ProxyMiddleware(null, Mock.Of<IProxy>(), Mock.Of<IConfiguration>(), null));
        }

        [Fact]
        public async void Invoke_CallsProxy()
        {
            Mock<IProxy> proxyMock = new Mock<IProxy>();
            Mock<IConfiguration> configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x[RemoteEndpointKey]).Returns(AValidEndpoint);
            Mock<HttpContext> contextMock = new Mock<HttpContext>();
            var response = Mock.Of<HttpResponse>();
            var request = Mock.Of<HttpRequest>(); ;
            contextMock.SetupGet(x => x.Request).Returns(request);
            contextMock.SetupGet(x => x.Response).Returns(response);

            ProxyMiddleware subject = new ProxyMiddleware(
                null, 
                proxyMock.Object, 
                configMock.Object, 
                Mock.Of<ILogger<ProxyMiddleware>>());

            await subject.Invoke(contextMock.Object);

            proxyMock.Verify(x => x.ProcessAsync(AValidEndpoint, request, response));
        }

        [Fact]
        public async void Invoke_Throws_Rethrows()
        {
            const string expectedMessage = "Failed to forward request";
            Mock<IProxy> proxyMock = new Mock<IProxy>();
            proxyMock.Setup(x => x.ProcessAsync(It.IsAny<string>(), It.IsAny<HttpRequest>(), It.IsAny<HttpResponse>()))
                .Throws(new Exception("FAIL"));
            Mock<IConfiguration> configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x[RemoteEndpointKey]).Returns(AValidEndpoint);
            Mock<ILogger<ProxyMiddleware>> loggerMock = new Mock<ILogger<ProxyMiddleware>>();
            Mock<HttpContext> contextMock = new Mock<HttpContext>();
            contextMock.SetupGet(x => x.Request).Returns(Mock.Of<HttpRequest>());
            contextMock.SetupGet(x => x.Response).Returns(Mock.Of<HttpResponse>());

            ProxyMiddleware subject = new ProxyMiddleware(
                null,
                proxyMock.Object,
                configMock.Object,
                loggerMock.Object);

            await Assert.ThrowsAsync<Exception>(() => subject.Invoke(contextMock.Object));
        }
    }
}
