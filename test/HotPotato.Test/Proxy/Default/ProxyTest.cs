using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HotPotato.Exceptions;
using HotPotato.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HotPotato.Proxy.Default
{
    public class ProxyTest
    {
        private const string AValidEndpoint = "http://foo/v1";
        private const string AValidPayload = "{foo:bar}";
        private const string AnInvalidMethod = "FOO";

        [Fact]
        public async void ProcessAsync_Delete_CallsClient()
        {
            const string method = "DELETE";
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Delete(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.Delete(It.IsAny<IHttpRequest>()));
        }

        [Fact]
        public async void ProcessAsync_Get_CallsClient()
        {
            const string method = "GET";
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Get(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.Get(It.IsAny<IHttpRequest>()));
        }

        [Fact]
        public async void ProcessAsync_Options_CallsClient()
        {
            const string method = "OPTIONS";
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Options(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.Options(It.IsAny<IHttpRequest>()));
        }

        [Fact]
        public async void ProcessAsync_Patch_CallsClient()
        {
            const string method = "PATCH";
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Patch(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            requestMock.SetupGet(x => x.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(AValidPayload)));
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.Patch(It.IsAny<IHttpRequest>()));
        }

        [Fact]
        public async void ProcessAsync_Post_CallsClient()
        {
            const string method = "POST";
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Post(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            requestMock.SetupGet(x => x.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(AValidPayload)));
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.Post(It.IsAny<IHttpRequest>()));
        }

        [Fact]
        public async void ProcessAsync_Put_CallsClient()
        {
            const string method = "PUT";
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Put(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(method);
            requestMock.SetupGet(x => x.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(AValidPayload)));
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

            clientMock.Verify(x => x.Put(It.IsAny<IHttpRequest>()));
        }

        [Fact]
        public async Task ProcessAsync_InvalidVerb_Throws()
        {
            Mock<IHttpResponse> internalResponseMock = new Mock<IHttpResponse>();
            internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
            Mock<IHttpClient> clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.Delete(It.IsAny<IHttpRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
            Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
            requestMock.SetupGet(x => x.Method).Returns(AnInvalidMethod);
            Mock<HttpResponse> responseMock = new Mock<HttpResponse>();

            Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>());

            await Assert.ThrowsAsync<InvalidHttpVerbException>(() => subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object));

        }
    }
}
