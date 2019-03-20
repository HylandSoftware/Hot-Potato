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
    public class StatusCodeValidatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void StatCodeValidator_GeneratesResponse()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    SwaggerOperation swagOp = new SwaggerOperation();
                    swagOp.Responses.Add("200", new SwaggerResponse());
                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specMeth = swagOp;

                    ResultCollector resColl = new ResultCollector();
                    StatusCodeValidator subject = new StatusCodeValidator(valPro, resColl);
                    subject.Validate(testPair);
                    
                    Assert.NotNull(valPro.specResp);
                }
            }
        }
        [Fact]
        public void StatCodeValidator_CreatesNotFoundResult()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    SwaggerOperation swagOp = new SwaggerOperation();
                    swagOp.Responses.Add("400", new SwaggerResponse());
                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    valPro.specMeth = swagOp;

                    ResultCollector resColl = new ResultCollector();
                    StatusCodeValidator subject = new StatusCodeValidator(valPro, resColl);
                    subject.Validate(testPair);

                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.MissingStatusCode, result.Reason);
                }
            }
        }
    }

}