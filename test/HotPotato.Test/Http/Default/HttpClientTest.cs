using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.Http.Default
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
            HttpRequest request = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint));

            HttpClient subject = new HttpClient(mockHttp.ToHttpClient());

            IHttpResponse response = await subject.SendAsync(request);

            mockHttp.VerifyNoOutstandingRequest();
        }
    }
}
