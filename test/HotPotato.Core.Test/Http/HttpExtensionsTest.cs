using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MSHTTP = Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Net.Http.Headers;
using System.Net.Http;
using Moq;
using System.IO;
using Microsoft.Extensions.Primitives;

namespace HotPotato.Core.Http
{
    public class HttpExtensionsTest
    {
        private const HttpStatusCode AVAlidStatusCode = HttpStatusCode.OK;
        private const string AValidKey = "x-header-key";
        private const string AnotherValidKey = "x-another-key";
        private const string AVAlidHeaderValue = "AValidHeaderValue";
        private const string AnotherHeaderValue = "AnotherHeaderValue";
        private readonly MediaTypeHeaderValue AValidMediaType = new MediaTypeHeaderValue("application/json");
        private const string AValidUri = "http://foo/";
        private const string AVAlidContent = "{'foo':'bar'}";
        private const string AValidContentType = "application/json";
        private const string GET = "GET";
        private const string POST = "POST";
            


        [Theory]
        [InlineData("connection")]
        [InlineData("content-length")]
        [InlineData("keep-alive")]
        [InlineData("host")]
        [InlineData("upgrade")]
        [InlineData("upgrade-insecure-requests")]
        [InlineData("Host")]
        [InlineData("Keep-Alive")]
        [InlineData("cOnNeCtIoN")]
        public void ToClientRequestMessage_HasExcludableHeaders_AreExcluded(string key)
        {
            IHttpRequest request = new Default.HttpRequest(new Uri(AValidUri));
            request.HttpHeaders.Add(key, AVAlidHeaderValue);

            HttpRequestMessage result = HttpExtensions.ToClientRequestMessage(request);
            Assert.False(result.Headers.TryGetValues(key, out _));
        }

        [Theory]
        [InlineData("X-Custom-Header")]
        [InlineData("Set-Cookie")]
        [InlineData("set-cookie")]
        [InlineData("Accept")]
        [InlineData("Referer")]
        [InlineData("Cache-Control")]
        public void ToClientRequestMessage_HasHeaders_AreIncluded(string key)
        {
            IHttpRequest request = new Default.HttpRequest(new Uri(AValidUri));
            request.HttpHeaders.Add(key, AVAlidHeaderValue);

            HttpRequestMessage result = HttpExtensions.ToClientRequestMessage(request);

            Assert.True(result.Headers.TryGetValues(key, out _));
        }

        [Fact]
        public void ToClientRequestMessage_NoContent_DoesNotSetContent()
        {
            IHttpRequest request = new Default.HttpRequest(new Uri(AValidUri));

            HttpRequestMessage result = HttpExtensions.ToClientRequestMessage(request);

            Assert.Null(result.Content);
        }

        [Fact]
        public void ToClientRequestMessage_HasContent_SetsContent()
        {
            IHttpRequest request = new Default.HttpRequest(new Uri(AValidUri));
            request.SetContent(AVAlidContent);

            HttpRequestMessage result = HttpExtensions.ToClientRequestMessage(request);

            Assert.Equal(request.Content, result.Content);
        }

        [Theory]
        [InlineData("Keep-Alive", "timeout=5, max=997")]
        [InlineData("Date", "Wed, 20 Jul 2016 16:06:00 GMT")]
        [InlineData("Server", "Kestrel")]
        [InlineData("X-Backend-Server", "hotpotato1.hyland.io")]
        [InlineData("x-custom-header", "custom-value")]
        public async Task ToClientResponseAsync_HasHeaders_AppliesHeaders(string key, string value)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(AVAlidStatusCode);
            httpResponseMessage.Headers.Add(key, value);

            IHttpResponse result = await HttpExtensions.ToClientResponseAsync(httpResponseMessage);

            Assert.True(result.Headers.ContainsKey(key));
        }

        [Fact]
        public async Task ToClientResponseAsync_NoContent_DoesNotSetContent()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(AVAlidStatusCode);

            IHttpResponse result = await HttpExtensions.ToClientResponseAsync(httpResponseMessage);

            Assert.Empty(result.Content);
        }

        [Fact]
        public async Task ToClientResponseAsync_HasContent_SetsContent()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(AVAlidStatusCode);
            httpResponseMessage.Content = new StringContent(AVAlidContent);

            IHttpResponse result = await HttpExtensions.ToClientResponseAsync(httpResponseMessage);

            Assert.NotNull(result.Content);
            Assert.Equal(Encoding.UTF8.GetBytes(AVAlidContent), result.Content);
        }

        [Fact]
        public async Task ToClientResponseAsync_HasContent_WithContentType_SetsContentType()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(AVAlidStatusCode);
            httpResponseMessage.Content = new StringContent(AVAlidContent, Encoding.UTF8, AValidContentType);

            IHttpResponse result = await HttpExtensions.ToClientResponseAsync(httpResponseMessage);

            Assert.Equal(AValidContentType, result.ContentType.Type);
        }

        [Fact]
        public async Task ToClientResponseAsync_HasContent_WithHeaders_SetsHeaders()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(AVAlidStatusCode);
            httpResponseMessage.Content = new StringContent(AVAlidContent);
            httpResponseMessage.Content.Headers.Add(AValidKey, AVAlidHeaderValue);

            IHttpResponse result = await HttpExtensions.ToClientResponseAsync(httpResponseMessage);

            Assert.True(result.Headers.ContainsKey(AValidKey));
            Assert.Equal(AVAlidHeaderValue, result.Headers[AValidKey][0]);
        }

        [Fact]
        public void ToProxyRequest_SetsRemoteEndpoint()
        {
            MSHTTP.HttpRequest request = new DefaultHttpRequest(new MSHTTP.DefaultHttpContext());
            request.Method = "GET";

            IHttpRequest result = HttpExtensions.ToProxyRequest(request, AValidUri);

            Assert.Equal(AValidUri, result.Uri.ToString());
        }

        [Fact]
        public void ToProxyRequest_WithHeaders_SetsHeaders()
        {
            MSHTTP.HttpRequest request = new DefaultHttpRequest(new MSHTTP.DefaultHttpContext());
            request.Method = GET;
            request.Headers.Add(AValidKey, AVAlidHeaderValue);
            request.Headers.Add(AnotherValidKey, AVAlidHeaderValue);

            IHttpRequest result = HttpExtensions.ToProxyRequest(request, AValidUri);

            Assert.NotNull(result.HttpHeaders);
            Assert.True(result.HttpHeaders.ContainsKey(AValidKey));
            Assert.True(result.HttpHeaders.ContainsKey(AnotherValidKey));
        }

        [Fact]
        public async Task ToProxyRequest_MethodWithPayload_SetsPayload()
        {
            byte[] content = Encoding.UTF8.GetBytes(AVAlidContent);
            using (MemoryStream contentStream = new MemoryStream(content.Length))
            {
                contentStream.Write(content, 0, content.Length);
                contentStream.Seek(0, SeekOrigin.Begin);
                Mock<MSHTTP.HttpRequest> request = new Mock<MSHTTP.HttpRequest>();
                request.SetupGet(r => r.Method).Returns(POST);
                request.SetupGet(r => r.Body).Returns(contentStream);

                IHttpRequest result = HttpExtensions.ToProxyRequest(request.Object, AValidUri);

                Assert.Equal(content, await result.Content.ReadAsByteArrayAsync());
            }
        }

        [Fact]
        public void ToProxyRequest_MethodWithPayload_NoBody_DoesNotSetPayload()
        {
            MSHTTP.HttpRequest request = new DefaultHttpRequest(new MSHTTP.DefaultHttpContext());
            request.Method = POST;

            IHttpRequest result = HttpExtensions.ToProxyRequest(request, AValidUri);

            Assert.Empty(result.Content.ReadAsByteArrayAsync().Result);
        }

        [Fact]
        public void ToProxyRequest_MethodWithoutPayload_DoesNotSetPayload()
        {
            MSHTTP.HttpRequest request = new DefaultHttpRequest(new MSHTTP.DefaultHttpContext());
            request.Method = GET;

            IHttpRequest result = HttpExtensions.ToProxyRequest(request, AValidUri);

            Assert.Null(result.Content);
        }

        [Fact]
        public async Task ToProxyResponseAsync_SetsStatusCode()
        {
            HttpHeaders headers = new HttpHeaders();
            IHttpResponse httpResponse = new Default.HttpResponse(AVAlidStatusCode, headers, new byte[0], AValidMediaType);
            MSHTTP.HttpResponse response = new DefaultHttpResponse(new MSHTTP.DefaultHttpContext());

            await HttpExtensions.ToProxyResponseAsync(httpResponse, response);

            Assert.Equal((int)AVAlidStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("x-custom-header")]
        [InlineData("keep-alive")]
        [InlineData("Set-Cookie")]
        [InlineData("X-Another-Custom")]
        public async Task ToProxyResponseAsync_HasHeaders_SetsHeaders(string key)
        {
            HttpHeaders headers = new HttpHeaders();
            headers.Add(key, AVAlidHeaderValue);
            IHttpResponse httpResponse = new Default.HttpResponse(AVAlidStatusCode, headers, new byte[0], AValidMediaType);
            MSHTTP.HttpResponse response = new DefaultHttpResponse(new MSHTTP.DefaultHttpContext());

            await HttpExtensions.ToProxyResponseAsync(httpResponse, response);

            Assert.True(response.Headers.ContainsKey(key));
        }

        [Fact]
        public async Task ToProxyResponseAsync_HasMultiValueHeaders_SetsHeaders()
        {
            HttpHeaders headers = new HttpHeaders();
            headers.Add(AValidKey, AVAlidHeaderValue);
            headers.Add(AValidKey, AnotherHeaderValue);
            IHttpResponse httpResponse = new Default.HttpResponse(AVAlidStatusCode, headers, new byte[0], AValidMediaType);
            MSHTTP.HttpResponse response = new DefaultHttpResponse(new MSHTTP.DefaultHttpContext());

            await HttpExtensions.ToProxyResponseAsync(httpResponse, response);

            Assert.True(response.Headers.TryGetValue(AValidKey, out StringValues result));
            Assert.Contains(AVAlidHeaderValue, (IEnumerable<string>)result);
            Assert.Contains(AnotherHeaderValue, (IEnumerable<string>)result);
        }

        [Theory]
        [InlineData("connection")]
        [InlineData("server")]
        [InlineData("transfer-encoding")]
        [InlineData("upgrade")]
        [InlineData("x-powered-by")]
        [InlineData("Connection")]
        [InlineData("sErVeR")]
        [InlineData("TRANSFER-ENCODING")]
        [InlineData("X-Powered-By")]
        public async Task ToProxyResponseAsync_HasExcludableHeaders_AreExcluded(string key)
        {
            HttpHeaders headers = new HttpHeaders();
            headers.Add(key, AVAlidHeaderValue);
            IHttpResponse httpResponse = new Default.HttpResponse(AVAlidStatusCode, headers, new byte[0], AValidMediaType);
            MSHTTP.HttpResponse response = new DefaultHttpResponse(new MSHTTP.DefaultHttpContext());

            await HttpExtensions.ToProxyResponseAsync(httpResponse, response);

            Assert.DoesNotContain(key, response.Headers.Keys);
        }

        [Fact]
        public async Task ToProxyResponseAsync_HasContent_WritesContent()
        {
            HttpHeaders headers = new HttpHeaders();
            IHttpResponse httpResponse = new Default.HttpResponse(AVAlidStatusCode, headers, Encoding.UTF8.GetBytes(AVAlidContent), AValidMediaType);
            Mock<MSHTTP.HttpResponse> response = new Mock<MSHTTP.HttpResponse>();
            response.Setup(r => r.Body).Returns(Mock.Of<Stream>).Verifiable();

            await HttpExtensions.ToProxyResponseAsync(httpResponse, response.Object);

            response.VerifyAll();
            
        }
    }
}
