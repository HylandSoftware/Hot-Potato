using HotPotato.Core.Http;
using Moq;
using System;
using Xunit;

namespace HotPotato.Core.Models
{
	public class HttpPairTest
	{

		[Fact]
		public void HttpPair_Constructor_ThrowsArgumentNullExceptionWithRequest()
		{
			Assert.Throws<ArgumentNullException>(() => new HttpPair(null, null));
		}

		[Fact]
		public void HttpPair_Constructor_ThrowsArgumentNullExceptionWithResponse()
		{
			IHotPotatoRequest request = Mock.Of<IHotPotatoRequest>();
			Assert.Throws<ArgumentNullException>(() => new HttpPair(request, null));
		}

		[Fact]
		public void HttpPair_Constructor_SetsRequestandResponse()
		{
			IHotPotatoRequest request = Mock.Of<IHotPotatoRequest>();
			IHotPotatoResponse response = Mock.Of<IHotPotatoResponse>();

			HttpPair subject = new HttpPair(request, response);

			Assert.Equal(request, subject.Request);
			Assert.Equal(response, subject.Response);
		}
	}
}
