using HotPotato.Core.Models;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using NSwag;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class PathValidatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void PathValidator_GeneratesPathItem()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    SwaggerDocument swagDoc = new SwaggerDocument();
                    swagDoc.Paths.Add("/workflow/life-cycles", new SwaggerPathItem());

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
        public void PathValidator_CreatesNotFoundResult()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    SwaggerDocument swagDoc = new SwaggerDocument();
                    swagDoc.Paths.Add("http://api.docs.hyland.io/deficiencies/deficiencies", new SwaggerPathItem());

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specDoc = swagDoc;

                    ResultCollector resColl = new ResultCollector();
                    PathValidator subject = new PathValidator(valPro, resColl);
                    subject.Validate(testPair);

                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.MissingPath, result.Reason);
                }
            }
        }
    }
}
