
using static HotPotato.IntegrationTestMethods;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using System;
using Xunit;

namespace HotPotato.OpenApi.SpecificationProvider
{
	public class SpecificationProviderTest
	{
		[SkippableFact]
		public void ISpecificationProvider_GetSpecDocument_ReturnsDocumentFromPath()
		{
			string specPath = SpecPath("specs/keyword/", "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.SkipMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			ISpecificationProvider subject = provider.GetService<ISpecificationProvider>();
			OpenApiDocument result = subject.GetSpecDocument();

			Assert.Equal(result.DocumentPath, specPath);
		}

		[Fact]
		public void ISpecificationProvider_GetSpecDocument_ThrowsInvalidOperationWithInvalidLocation()
		{
			ServiceProvider provider = GetServiceProvider(string.Empty);

			ISpecificationProvider subject = provider.GetService<ISpecificationProvider>();

			Action result = () => subject.GetSpecDocument();
			Assert.Throws<InvalidOperationException>(result);
		}
	}
}
