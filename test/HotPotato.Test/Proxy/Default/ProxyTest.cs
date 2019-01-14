using HotPotato.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HotPotato.Proxy.Default
{
    public class ProxyTest
    {
        private const string AValidEndpoint = "http://foo/v1";
        private const string AValidPayload = "{foo:bar}";
        private const string AnInvalidMethod = "FOO";
        private const string DELETE = "DELETE";
        private const string GET = "GET";
        private const string OPTIONS = "OPTIONS";
        private const string PATCH = "PATCH";
        private const string POST = "POST";
        private const string PUT = "PUT";

        [Theory]
        [InlineData(DELETE)]
        [InlineData(GET)]
        [InlineData(OPTIONS)]
        [InlineData(PATCH)]
        [InlineData(POST)]
        [InlineData(PUT)]
        public async void ProcessAsync_CallsClient(string method)
        {
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            internalResponseMock.SetupGet(x => x.Headers).Returns(new HttpHeaders());
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.SendAsync(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            var requestHeaders = new HeaderDictionary();
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            requestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
            var responseHeaders = new HeaderDictionary();
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(x => x.Headers).Returns(responseHeaders);

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.SendAsync(It.IsAny<IHttpRequest>()));
        }
    }
}
