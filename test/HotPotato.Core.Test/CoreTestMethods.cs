using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotPotato.Core
{
	public static class CoreTestMethods
	{
		public static Mock<HttpMessageHandler> GetMockHandler(HttpStatusCode statusCode, string contentString = "")
		{
			HttpContent expectedContent = new StringContent(contentString, Encoding.UTF8, "application/json");

			var mockHandler = new Mock<HttpMessageHandler>();
			mockHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage()
				{
					StatusCode = statusCode,
					Content = expectedContent,
				})
				.Verifiable();

			return mockHandler;
		}

		public static HttpClient ToHttpClient(this HttpMessageHandler @this)
		{
			return new HttpClient(@this);
		}
	}
}
