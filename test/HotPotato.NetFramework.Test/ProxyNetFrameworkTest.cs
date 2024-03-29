using HotPotato.Core.Http;
using HotPotato.Core.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HotPotato.Core.Proxy.Default
{
	public class ProxyNetFrameworkTest
	{
		private const string AValidEndpoint = "http://foo/v1";
		private const string DELETE = "DELETE";
		private const string GET = "GET";
		private const string OPTIONS = "OPTIONS";
		private const string PATCH = "PATCH";
		private const string POST = "POST";
		private const string PUT = "PUT";

		[Test]
		[TestCase(DELETE)]
		[TestCase(GET)]
		[TestCase(OPTIONS)]
		[TestCase(PATCH)]
		[TestCase(POST)]
		[TestCase(PUT)]
		public async Task ProcessAsync_CallsClient(string method)
		{
			Mock<IHotPotatoResponse> internalResponseMock = new Mock<IHotPotatoResponse>();
			internalResponseMock.SetupGet(x => x.StatusCode).Returns(System.Net.HttpStatusCode.OK);
			internalResponseMock.SetupGet(x => x.Headers).Returns(new HttpHeaders());
			Mock<IHotPotatoClient> clientMock = new Mock<IHotPotatoClient>();
			clientMock.Setup(x => x.SendAsync(It.IsAny<IHotPotatoRequest>())).Returns(Task.FromResult(internalResponseMock.Object));
			var requestHeaders = new HeaderDictionary();
			Mock<HttpRequest> requestMock = new Mock<HttpRequest>();
			requestMock.SetupGet(x => x.Method).Returns(method);
			requestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
			var responseHeaders = new HeaderDictionary();
			Mock<HttpResponse> responseMock = new Mock<HttpResponse>();
			responseMock.SetupGet(x => x.Headers).Returns(responseHeaders);

			Proxy subject = new Proxy(clientMock.Object, Mock.Of<ILogger<Proxy>>(), Mock.Of<IProcessor>());

			await subject.ProcessAsync(AValidEndpoint, requestMock.Object, responseMock.Object);

			clientMock.Verify(x => x.SendAsync(It.IsAny<IHotPotatoRequest>()));
		}
	}
}
