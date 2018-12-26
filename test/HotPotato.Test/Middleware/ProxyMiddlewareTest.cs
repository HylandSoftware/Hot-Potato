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
    }
}
