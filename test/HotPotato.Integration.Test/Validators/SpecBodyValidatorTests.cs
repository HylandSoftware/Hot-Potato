
using static HotPotato.IntegrationTestMethods;
using HotPotato.OpenApi.Locators.NSwag;
using HotPotato.OpenApi.Results;
using static HotPotato.OpenApi.Results.ResultsMethods;
using HotPotato.OpenApi.Validators;
using Newtonsoft.Json;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Models;
using HotPotato.Core.Http;

namespace HotPotato.Http.Default
{

    public class SpecBodyValidatorTests
    {
        [Theory]
        [ClassData(typeof(SpecValidationTestData))]
        public async void Locator_ReturnsBodyValidator(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
        {

            string bodyString = JsonConvert.SerializeObject(bodyJson);
            HttpContent content = new StringContent(bodyString, Encoding.UTF8, contentType);
            
            HttpResponseMessage testRespMsg = new HttpResponseMessage();
            testRespMsg.StatusCode = statusCode;
            testRespMsg.Content = content;
            var testResponse = await testRespMsg.ToClientResponseAsync();

            using (HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    string specPath = SpecPath(specSubPath, "specification.yaml");
                    Task<SwaggerDocument> swagTask = FromFileAsync(specPath);
                    SwaggerDocument swagDoc = swagTask.Result;

                    Locator subject = new Locator(swagDoc, new PathLocator(), new MethodLocator(), new StatusCodeLocator());
                    Tuple<IBodyValidator, IHeaderValidator> valTup = subject.GetValidator(testPair);
                    Result result = valTup.Item1.Validate(bodyString);
                    Assert.Contains("is valid", result.Message);
                }
            }

        }

        [Theory]
        [ClassData(typeof(SpecValidationNegTestData))]
        public async void Locator_ReturnsBodyValidator_InvalidSchema(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
        {

            string bodyString = JsonConvert.SerializeObject(bodyJson);
            HttpContent content = new StringContent(bodyString, Encoding.UTF8, contentType);

            HttpResponseMessage testRespMsg = new HttpResponseMessage();
            testRespMsg.StatusCode = statusCode;
            testRespMsg.Content = content;

            var testResponse = await testRespMsg.ToClientResponseAsync();

            using (HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI)))
            {
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {

                    string specPath = SpecPath(specSubPath, "specification.yaml");
                    Task<SwaggerDocument> swagTask = FromFileAsync(specPath);
                    SwaggerDocument swagDoc = swagTask.Result;

                    Locator subject = new Locator(swagDoc, new PathLocator(), new MethodLocator(), new StatusCodeLocator());
                    Tuple<IBodyValidator, IHeaderValidator> valTup = subject.GetValidator(testPair);
                    Result result = valTup.Item1.Validate(bodyString);

                    Assert.Equal(ValidationErrorKind.DateTimeExpected, GetInvalidReasons(result)[0].Kind);
                    Assert.Equal(ValidationErrorKind.IntegerExpected, GetInvalidReasons(result)[1].Kind);
                    Assert.Contains("is invalid", result.Message); 
                }
            }

        }
    }
}
