using static HotPotato.IntegrationTestMethods;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class SpecPathValTest
	{
		[SkippableFact]
		public void PathValidator_GeneratesSpecPathWithParam()
		{
			string specPath = SpecPath("specs/keyword/", "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
			OpenApiDocument swagDoc = specPro.GetSpecDocument();

			PathValidator subject = new PathValidator("http://api.docs.hyland.io/keyword/keyword-type-groups/48732/keyword-types");

			Assert.True(subject.Validate(swagDoc));
			Assert.Equal("get", subject.Result.Keys.ElementAt(0).ToLower());
		}

		[SkippableFact]
		public void PathValidator_GeneratesSpecPathWithoutParam()
		{
			string specPath = SpecPath("specs/onbase-workflow/", "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
			OpenApiDocument swagDoc = specPro.GetSpecDocument();

			PathValidator subject = new PathValidator("https://api.hyland.com/onbase-workflow/life-cycles");

			Assert.True(subject.Validate(swagDoc));
			Assert.Equal("get", subject.Result.Keys.ElementAt(0).ToLower());
		}
	}
}
