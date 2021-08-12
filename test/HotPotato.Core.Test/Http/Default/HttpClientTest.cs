using static HotPotato.Core.CoreTestMethods;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HotPotato.Core.Http.Default
{
    public class HttpClientTest
    {
        private const string AValidEndpoint = "http://localhost/api";

        [Fact]
        public void Constructor_ThrowsArgumentNullExceptionWithClient()
        {
            Assert.Throws<ArgumentNullException>(() => new HotPotatoClient(null));
        }

        [Fact]
        public async Task SendAsync_ExecutesRequest()
        {
            var handlerMock = GetMockHandler(HttpStatusCode.OK);
            Uri endpointUri = new Uri(AValidEndpoint);

            using (HttpMessageHandler mockHandler = handlerMock.Object)
            using (HotPotatoRequest request = new HotPotatoRequest(HttpMethod.Get, endpointUri))
            {
                HotPotatoClient subject = new HotPotatoClient(mockHandler.ToHttpClient());

                IHotPotatoResponse response = await subject.SendAsync(request);

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
        public async Task SendAsync_ResponseContainsContent_WithType()
        {
            string expectString = "{\"id\":\"string\"}";

            using (HttpContent expectContent = new StringContent(expectString, Encoding.UTF8, "application/json"))
            using (HttpMessageHandler mockHandler = GetMockHandler(HttpStatusCode.OK, expectString).Object)
            using (HotPotatoRequest request = new HotPotatoRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                request.SetContent(expectString);

                HotPotatoClient subject = new HotPotatoClient(mockHandler.ToHttpClient());

                IHotPotatoResponse response = await subject.SendAsync(request);
                string bodyString = response.ToBodyString();

                Assert.Equal("utf-8", response.ContentType.CharSet);
                Assert.Equal("application/json", response.ContentType.Type);
                Assert.Equal(bodyString, expectString);
            }
        }
    }
}
