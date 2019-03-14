using HotPotato.Core.Models;
using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using NJsonSchema;
using NSwag;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class BodyValidatorTest
    {
        private const string AValidBody = "{'foo': '1'}";
        private const string AnInvalidBody = "{'foo': 'abc'}";
        private const string AValidSchema = @"{'type': 'integer'}";
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";

        [Fact]
        public async void Validate_ValidBody()
        {
            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(HttpStatusCode.OK))
            {
                testRespMsg.Content = new StringContent(AValidBody, Encoding.UTF8, "application/json");
                var testResponse = await testRespMsg.ToClientResponseAsync();

                using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
                {
                    using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                    {
                        JsonSchema4 schema = JsonSchema4.CreateAnySchema();
                        ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                        SwaggerResponse swagResp = new SwaggerResponse();
                        swagResp.ActualResponse.Schema = schema;
                        valPro.specResp = swagResp;

                        ResultCollector resColl = new ResultCollector();

                        BodyValidator bodyVal = new BodyValidator(valPro, resColl);
                        bodyVal.Validate(testPair);
                        Result result = resColl.Results.ElementAt(0);

                        Assert.Equal(State.Pass, result.State);
                    }
                } 
            }
        }

        [Fact]
        public async void Validate_InvalidBody()
        {
            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(HttpStatusCode.OK))
            {
                testRespMsg.Content = new StringContent(AValidBody, Encoding.UTF8, "application/json");
                var testResponse = await testRespMsg.ToClientResponseAsync();

                using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
                {
                    using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                    {
                        JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
                        ValidationProvider valPro = new ValidationProvider(Mock.Of<ISpecificationProvider>());
                        SwaggerResponse swagResp = new SwaggerResponse();
                        swagResp.ActualResponse.Schema = schema;
                        valPro.specResp = swagResp;

                        ResultCollector resColl = new ResultCollector();

                        BodyValidator bodyVal = new BodyValidator(valPro, resColl);
                        bodyVal.Validate(testPair);
                        Result result = resColl.Results.ElementAt(0);

                        Assert.Equal(State.Fail, result.State);
                        Assert.Equal(ValidationErrorKind.IntegerExpected, result.ValidationErrors[0].Kind);
                    }
                }
            }
        }
    }
}
