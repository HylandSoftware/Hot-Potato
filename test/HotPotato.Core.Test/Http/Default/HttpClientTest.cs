using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HotPotato.Core.Http.Default
{
    public class HttpClientTest
    {
        private const string AValidEndpoint = "http://localhost/api";

        [Fact]
        public async void SendAsync_ExecutesRequest()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(AValidEndpoint)
                .Respond(HttpStatusCode.OK);
            using (HttpRequest request = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                HttpClient subject = new HttpClient(mockHttp.ToHttpClient());

                IHttpResponse response = await subject.SendAsync(request);

                mockHttp.VerifyNoOutstandingRequest();
            }
        }

        [Fact]
        public async void SendAsync_ResponseContainsContent_WithType()
        {
            string expectString = "{\"id\":\"string\"}";

            HttpContent expectContent = new StringContent(expectString, Encoding.UTF8, "application/json");

            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            mockHttp.When(AValidEndpoint)
                .Respond(HttpStatusCode.OK).Respond(expectContent);

            using (HttpRequest request = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                request.SetContent(expectString);

                HttpClient subject = new HttpClient(mockHttp.ToHttpClient());

                IHttpResponse response = await subject.SendAsync(request);
                string bodyString = response.ToBodyString();

                Assert.Equal("utf-8", response.ContentType.CharSet);
                Assert.Equal("application/json", response.ContentType.MediaType);
                Assert.Equal(bodyString, expectString);
            }
        }
    }
}
