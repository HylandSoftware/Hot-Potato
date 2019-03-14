using HotPotato.Core.Models;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using Newtonsoft.Json;
using NJsonSchema;
using NSwag;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class HeaderValidatorTest
    {
        private const string AValidHeaderKey = "X-Header-Key";
        private const string AValidHeaderValue = "value";
        private const string AValidSchema = @"{'type': 'integer'}";
        private const string AnInvalidValue = "invalidValue";
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";

        [Fact]
        public void Validate_KeyNotFound()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    SwaggerResponse swagResp = new SwaggerResponse();
                    swagResp.Headers.Add(AValidHeaderKey, new JsonSchema4());
                    valPro.specResp = swagResp;

                    ResultCollector resColl = new ResultCollector();

                    HeaderValidator headVal = new HeaderValidator(valPro, resColl);
                    headVal.Validate(testPair);
                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.MissingHeaders, result.Reason);
                }
            }
        }

        [Fact]
        public void Validate_ValidSchema()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, new Core.Http.HttpHeaders());

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    testPair.Response.Headers.Add(AValidHeaderKey, AValidHeaderValue);

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    SwaggerResponse swagResp = new SwaggerResponse();
                    swagResp.Headers.Add(AValidHeaderKey, JsonSchema4.CreateAnySchema());
                    valPro.specResp = swagResp;

                    ResultCollector resColl = new ResultCollector();

                    HeaderValidator headVal = new HeaderValidator(valPro, resColl);
                    headVal.Validate(testPair);
                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Pass, result.State);
                }
            }
        }

        [Fact]
        public void Validate_InvalidSchema()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, new Core.Http.HttpHeaders());

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    testPair.Response.Headers.Add(AValidHeaderKey, AnInvalidValue);

                    ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                    SwaggerResponse swagResp = new SwaggerResponse();
                    swagResp.Headers.Add(AValidHeaderKey, JsonSchema4.FromJsonAsync(AValidSchema).Result);
                    valPro.specResp = swagResp;

                    ResultCollector resColl = new ResultCollector();

                    HeaderValidator headVal = new HeaderValidator(valPro, resColl);
                    headVal.Validate(testPair);
                    Result result = resColl.Results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.InvalidHeaders, result.Reason);
                }
            }
        }
    }
}
