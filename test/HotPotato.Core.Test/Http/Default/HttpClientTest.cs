using static HotPotato.Core.CoreTestMethods;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace HotPotato.Core.Http.Default
{
    public class HttpClientTest
    {
        private const string AValidEndpoint = "http://localhost/api";

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionWithClient()
        {
            Action subject = () => new HttpClient(null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public async void SendAsync_ExecutesRequest()
        {
            var handlerMock = GetMockHandler(HttpStatusCode.OK);
            HttpMessageHandler mockHandler = handlerMock.Object;

            Uri endpointUri = new Uri(AValidEndpoint);

            using (HttpRequest request = new HttpRequest(HttpMethod.Get, endpointUri))
            {
                HttpClient subject = new HttpClient(mockHandler.ToHttpClient());

                IHttpResponse response = await subject.SendAsync(request);

                Assert.NotNull(response);

                handlerMock.Protected().Verify("SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri == endpointUri),
                    ItExpr.IsAny<CancellationToken>());
            }
        }

        [Fact]
        public async void SendAsync_ResponseContainsContent_WithType()
        {
            string expectString = "{\"id\":\"string\"}";
            HttpContent expectContent = new StringContent(expectString, Encoding.UTF8, "application/json");

            HttpMessageHandler mockHandler = GetMockHandler(HttpStatusCode.OK, expectString).Object;

            using (HttpRequest request = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                request.SetContent(expectString);

                HttpClient subject = new HttpClient(mockHandler.ToHttpClient());

                IHttpResponse response = await subject.SendAsync(request);
                string bodyString = response.ToBodyString();

                Assert.Equal("utf-8", response.ContentType.CharSet);
                Assert.Equal("application/json", response.ContentType.Type);
                Assert.Equal(bodyString, expectString);
            }
        }
    }
}
