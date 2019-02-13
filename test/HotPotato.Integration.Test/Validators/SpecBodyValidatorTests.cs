
using HotPotato.OpenApi.Locators.NSwag;
using HotPotato.Models;
using HotPotato.Validators;
using Newtonsoft.Json;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            
            HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI));

            HttpResponseMessage testRespMsg = new HttpResponseMessage();
            testRespMsg.StatusCode = statusCode;
            testRespMsg.Content = content;
            var testResponse = await testRespMsg.ConvertResponse();
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            string specPath = Path.Combine(Environment.CurrentDirectory, specSubPath, "specification.yaml");
            Task<SwaggerDocument> swagTask = FromFileAsync(specPath);
            SwaggerDocument swagDoc = swagTask.Result;

            Locator subject = new Locator(swagDoc, new PathLocator(), new MethodLocator(), new StatusCodeLocator());
            Tuple<IBodyValidator, IHeaderValidator> valTup = subject.GetValidator(testPair);
            Results.Result result = valTup.Item1.Validate(bodyString);
            Assert.Contains("is valid", result.Message);

        }

        //TODO: Use this theory to validate that the correct error messages are being returned once our custom Results object is made
        [Theory]
        [ClassData(typeof(SpecValidationNegTestData))]
        public async void Locator_ReturnsBodyValidator_InvalidSchema(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
        {

            string bodyString = JsonConvert.SerializeObject(bodyJson);
            HttpContent content = new StringContent(bodyString, Encoding.UTF8, contentType);

            HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI));

            HttpResponseMessage testRespMsg = new HttpResponseMessage();
            testRespMsg.StatusCode = statusCode;
            testRespMsg.Content = content;
            var testResponse = await testRespMsg.ConvertResponse();
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            string specPath = Path.Combine(Environment.CurrentDirectory, specSubPath, "specification.yaml");
            Task<SwaggerDocument> swagTask = FromFileAsync(specPath);
            SwaggerDocument swagDoc = swagTask.Result;

            Locator subject = new Locator(swagDoc, new PathLocator(), new MethodLocator(), new StatusCodeLocator());
            Tuple<IBodyValidator, IHeaderValidator> valTup = subject.GetValidator(testPair);
            Results.Result result = valTup.Item1.Validate(bodyString);
            Assert.Contains("is invalid", result.Message);

        }
    }
}
