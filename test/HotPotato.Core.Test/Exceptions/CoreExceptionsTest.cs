using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace HotPotato.Core
{
    public class CoreExceptionsTest
    {
        [Fact]
        public void HttpClient_Constructor_ThrowsArgumentNullExceptionWithClient()
        {
            Action subject = () => new Http.Default.HttpClient(null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpExtensions_ToClientRequestMessage_ThrowsArgumentNullExceptionWithRequest()
        {
            IHttpRequest request = null;
            Action subject = () => request.ToClientRequestMessage();
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpExtensions_ToClientResponseMessage_ThrowsArgumentNullExceptionWithResponseMessage()
        {
            HttpResponseMessage respMsg = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await respMsg.ToClientResponseAsync());
        }

        [Fact]
        public void HttpExtensions_ToProxyRequest_ThrowsArgumentNullExceptionWithMsHttpRequest()
        {
            Microsoft.AspNetCore.Http.HttpRequest request = null;
            Action subject = () => request.ToProxyRequest(null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpExtensions_ToProxyRequest_ThrowsArgumentNullExceptionWithRemoteEndpoint()
        {
            Microsoft.AspNetCore.Http.HttpRequest request = Mock.Of<Microsoft.AspNetCore.Http.HttpRequest>();
            Action subject = () => request.ToProxyRequest(null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpExtensions_ToProxyResponseAsync_ThrowsArgumentNullExceptionWithIHttpResponse()
        {
            IHttpResponse response = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await response.ToProxyResponseAsync(null));
        }

        [Fact]
        public void HttpExtensions_ToProxyResponseAsync_ThrowsArgumentNullExceptionWithMsHttpResponse()
        {
            IHttpResponse response = Mock.Of<IHttpResponse>();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await response.ToProxyResponseAsync(null));
        }

        [Fact]
        public void HttpExtensions_BuildUri_ThrowsArgumentNullExceptionWithMsHttpRequest()
        {
            Microsoft.AspNetCore.Http.HttpRequest request = null;
            Action subject = () => request.BuildUri(null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpExtensions_BuildUri_ThrowsArgumentNullExceptionWithRemoteEndpoint()
        {
            Microsoft.AspNetCore.Http.HttpRequest request = Mock.Of<Microsoft.AspNetCore.Http.HttpRequest>();
            Action subject = () => request.BuildUri(null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpPair_Constructor_ThrowsArgumentNullExceptionWithRequest()
        {
            Action subject = () => new HttpPair(null, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpPair_Constructor_ThrowsArgumentNullExceptionWithResponse()
        {
            IHttpRequest request = Mock.Of<IHttpRequest>();
            Action subject = () => new HttpPair(request, null);
            Assert.Throws<ArgumentNullException>(subject);
        }
        [Fact]
        public void Proxy_Constructor_ThrowsArgumentNullExceptionWithClient()
        {
            Action subject = () => new Proxy.Default.Proxy(null, null, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void Proxy_Constructor_ThrowsArgumentNullExceptionWithConfig()
        {
            Action subject = () => new Proxy.Default.Proxy(Mock.Of<IHttpClient>(), null, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void Proxy_Constructor_ThrowsArgumentNullExceptionWithLogger()
        {
            Action subject = () => new Proxy.Default.Proxy(null, Mock.Of<ILogger<Proxy.Default.Proxy>>(), Mock.Of<IProcessor>());
            Assert.Throws<ArgumentNullException>(subject);
        }
    }
}
