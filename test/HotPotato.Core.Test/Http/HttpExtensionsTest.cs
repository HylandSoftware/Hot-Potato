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

namespace HotPotato.Core.Http
{
    public class HttpExtensionsTest
    {
        private const HttpStatusCode AVAlidStatusCode = HttpStatusCode.OK;
        private const string AVAlidHeaderValue = "AValidHeaderValue";
        private readonly MediaTypeHeaderValue AValidMediaType = new MediaTypeHeaderValue("application/json");
        private const string AValidUri = "http://foo";
        private const string AVAlidContent = "AValidContent";


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


    }
}
