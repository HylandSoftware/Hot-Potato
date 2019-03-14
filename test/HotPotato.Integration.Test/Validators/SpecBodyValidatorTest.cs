
using static HotPotato.IntegrationTestMethods;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSwag;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using HotPotato.OpenApi.SpecificationProvider;
using HotPotato.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HotPotato.Http.Default
{

    public class SpecBodyValidatorTest
    {
        [Theory]
        [ClassData(typeof(SpecValidationTestData))]
        public async void BodyValidator_ValidatesWithSpecData(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            string bodyString = JsonConvert.SerializeObject(bodyJson);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
                var testResponse = await testRespMsg.ToClientResponseAsync();

                using (HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI)))
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
                    SwaggerDocument swagDoc = specPro.GetSpecDocument();

                    IProcessor processor = provider.GetService<IProcessor>();
                    processor.Process(testPair);

                    IResultCollector collector = provider.GetService<IResultCollector>();

                    List<Result> results = collector.Results;
                    Result result = results.ElementAt(0);

                    Assert.Equal(State.Pass, result.State);

                } 
            }
        }

        [Theory]
        [ClassData(typeof(SpecValidationNegTestData))]
        public async void BodyValidator_ValidatesWithSpecData_InvalidSchema(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
        {

            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            string bodyString = JsonConvert.SerializeObject(bodyJson);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType); ;
                var testResponse = await testRespMsg.ToClientResponseAsync();

                using (HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI)))
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {

                    ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
                    SwaggerDocument swagDoc = specPro.GetSpecDocument();

                    IProcessor processor = provider.GetService<IProcessor>();
                    processor.Process(testPair);

                    IResultCollector collector = provider.GetService<IResultCollector>();

                    List<Result> results = collector.Results;
                    Result result = results.ElementAt(0);

                    Assert.Equal(Reason.InvalidBody, result.Reason);
                    Assert.Equal(ValidationErrorKind.DateTimeExpected, result.ValidationErrors[0].Kind);
                    Assert.Equal(ValidationErrorKind.IntegerExpected, result.ValidationErrors[1].Kind);
                }
            }

        }
    }
}
