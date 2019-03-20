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
    public class MethodValidatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void MethodValidator_GeneratesOperation()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    SwaggerPathItem swagPath = new SwaggerPathItem();
                    swagPath.Add("GET", new SwaggerOperation());

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specPath = swagPath;

                    ResultCollector resColl = new ResultCollector();
                    MethodValidator subject = new MethodValidator(valPro, resColl);
                    subject.Validate(testPair);

                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.MissingMethod, result.Reason);
                }
            }
        }
        [Fact]
        public void MethodValidator_CreatesNotFoundResult()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    SwaggerPathItem swagPath = new SwaggerPathItem();
                    swagPath.Add("TRACE", new SwaggerOperation());

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specPath = swagPath;

                    ResultCollector resColl = new ResultCollector();
                    MethodValidator subject = new MethodValidator(valPro, resColl);
                    subject.Validate(testPair);

                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.MissingMethod, result.Reason);
                }
            }
        }
    }
}
