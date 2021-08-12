using HotPotato.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace HotPotato.OpenApi.SpecificationProvider
{
	public class SpecificationProviderTest
	{
		private const string ANonExistentSpecLocation = "https://raw.githubusercontent.com/ASpecThatDoesNotExist.yaml";

		[Fact]
		public void SpecificationProvider_Constructor_ThrowsArgumentNullExceptionWithConfig()
		{
			ILogger<SpecificationProvider> mockLogger = Mock.Of<ILogger<SpecificationProvider>>();
			Assert.Throws<ArgumentNullException>(() => new SpecificationProvider(null, mockLogger));
		}

		[Fact]
		public void SpecificationProvider_Constructor_ThrowsArgumentNullExceptionWithLogger()
		{
			IConfiguration mockConfig = Mock.Of<IConfiguration>();
			Assert.Throws<ArgumentNullException>(() => new SpecificationProvider(mockConfig, null));
		}

		[Fact]
		public void SpecificationProvider_GetSpecDocument_ThowsSpecNotFound_WithANonExistentSpecLocation()
		{
			ILogger<SpecificationProvider> mockLogger = Mock.Of<ILogger<SpecificationProvider>>();

			Dictionary<string, string> nonExistentSpecLocation = new Dictionary<string, string>
			{
				{ "SpecLocation", ANonExistentSpecLocation }
			};

			IConfiguration config = new ConfigurationBuilder()
				.AddInMemoryCollection(nonExistentSpecLocation)
				.Build();

			SpecificationProvider subject = new SpecificationProvider(config, mockLogger);

			var result = Assert.Throws<AggregateException>(() => subject.GetSpecDocument());
			Assert.IsType<SpecNotFoundException>(result.InnerException);
		}

		[Fact]
		public void SpecificationProvider_GetSpecDocument_ThrowsInvalidOperation_WithAnInvalidSpecLocation()
		{
			ILogger<SpecificationProvider> mockLogger = Mock.Of<ILogger<SpecificationProvider>>();

			Dictionary<string, string> invalidSpecLocation = new Dictionary<string, string>
			{
				{ "SpecLocation", "" }
			};

			IConfiguration config = new ConfigurationBuilder()
				.AddInMemoryCollection(invalidSpecLocation)
				.Build();

			SpecificationProvider subject = new SpecificationProvider(config, mockLogger);

			Assert.Throws<InvalidOperationException>(() => subject.GetSpecDocument());
		}
	}
}
