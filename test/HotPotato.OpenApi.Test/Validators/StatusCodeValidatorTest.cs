using HotPotato.Core.Models;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using NSwag;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class StatusCodeValidatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void StatCodeValidator_SetsValData()
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
    }

}