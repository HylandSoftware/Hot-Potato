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
        [Fact]
        public void PathValidator_GeneratesSpecPathWithParam()
        {
            string specPath = SpecPath("specs/keyword/", "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
            OpenApiDocument swagDoc = specPro.GetSpecDocument();

            ResultCollector resColl = new ResultCollector();

            PathValidator subject = new PathValidator("http://api.docs.hyland.io/keyword/keyword-type-groups/48732/keyword-types");

            Assert.True(subject.Validate(swagDoc));
            Assert.Equal("get", subject.Result.Keys.ElementAt(0).ToLower());
        }

        [Fact]
        public void PathValidator_GeneratesSpecPathWithoutParam()
        {
            string specPath = SpecPath("specs/workflow/", "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
            OpenApiDocument swagDoc = specPro.GetSpecDocument();

            ResultCollector resColl = new ResultCollector();

            PathValidator subject = new PathValidator("https://api.hyland.com/workflow/life-cycles");

            Assert.True(subject.Validate(swagDoc));
            Assert.Equal("get", subject.Result.Keys.ElementAt(0).ToLower());
        }
    }
}
