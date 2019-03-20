using static HotPotato.IntegrationTestMethods;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NSwag;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using HotPotato.Core.Http.Default;

namespace HotPotato.OpenApi.Validators
{
    public class SpecPathValTest
    {
        [Fact]
        public void PathValidator_GeneratesSpecPathWithParam()
        {
            string endpointWithPar = "http://api.docs.hyland.io/keyword/keyword-type-groups/48732/keyword-types";
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithPar)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    string specPath = SpecPath("specs/keyword/", "specification.yaml");
                    ServiceProvider provider = GetServiceProvider(specPath);

                    ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
                    SwaggerDocument swagDoc = specPro.GetSpecDocument();

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specDoc = swagDoc;

                    ResultCollector resColl = new ResultCollector();

                    PathValidator subject = new PathValidator(valPro, resColl);
                    subject.Validate(testPair);

                    Assert.NotNull(valPro.specPath);
                }
            }
        }

        [Fact]
        public void PathValidator_GeneratesSpecPathWithoutParam()
        {
            string endpointWithoutPar = "https://api.hyland.com/workflow/life-cycles";
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithoutPar)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    string specPath = SpecPath("specs/workflow/", "specification.yaml");
                    ServiceProvider provider = GetServiceProvider(specPath);

                    ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
                    SwaggerDocument swagDoc = specPro.GetSpecDocument();

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specDoc = swagDoc;

                    ResultCollector resColl = new ResultCollector();

                    PathValidator subject = new PathValidator(valPro, resColl);
                    subject.Validate(testPair);

                    Assert.NotNull(valPro.specPath);
                }
            }
        }

    }

}
