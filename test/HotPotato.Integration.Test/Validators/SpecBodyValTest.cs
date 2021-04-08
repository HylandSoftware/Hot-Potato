
using static HotPotato.IntegrationTestMethods;
using HotPotato.OpenApi.Results;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSwag;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using HotPotato.OpenApi.SpecificationProvider;
using HotPotato.OpenApi.Models;
using HotPotato.Http.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HotPotato.OpenApi.Validators
{

    public class SpecBodyValTest
    {
        [Theory]
        [ClassData(typeof(SpecBodyValidTestData))]
        public async void BodyValidator_CreatesValidResult(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
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
                    OpenApiDocument swagDoc = specPro.GetSpecDocument();

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
        [ClassData(typeof(SpecBodyInvalidTestData))]
        public async void BodyValidator_CreatesInvalidResult(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, 
            string endpointURI, string contentType, object bodyJson, ValidationErrorKind expectedKind1, ValidationErrorKind expectedKind2)
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
                    OpenApiDocument swagDoc = specPro.GetSpecDocument();

                    IProcessor processor = provider.GetService<IProcessor>();
                    processor.Process(testPair);

                    IResultCollector collector = provider.GetService<IResultCollector>();

                    List<Result> results = collector.Results;
                    FailResult result = (FailResult)results.ElementAt(0);

                    Assert.Equal(Reason.InvalidBody, result.Reasons.ElementAt(0));
                    Assert.Equal(expectedKind1, result.ValidationErrors[0].Kind);
                    Assert.Equal(expectedKind2, result.ValidationErrors[1].Kind);
                }
            }

        }

        [Theory]
        [ClassData(typeof(CustomSpecTestData))]
        public async void BodyValidator_CreatesValidResultWithDiffTypes(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, string bodyString)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
                var testResponse = await testRespMsg.ToClientResponseAsync();

                using (HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI)))
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
                    OpenApiDocument swagDoc = specPro.GetSpecDocument();

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
        [ClassData(typeof(CustomSpecNegTestData))]
        public async void BodyValidator_CreatesInvalidResultWithDiffTypes(string specSubPath, HttpMethod reqMethod, 
            HttpStatusCode statusCode, string endpointURI, string contentType, string bodyString, ValidationErrorKind errorKind)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
                var testResponse = await testRespMsg.ToClientResponseAsync();

                using (HttpRequest testRequest = new HttpRequest(reqMethod, new Uri(endpointURI)))
                using (HttpPair testPair = new HttpPair(testRequest, testResponse))
                {
                    ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
                    OpenApiDocument swagDoc = specPro.GetSpecDocument();

                    IProcessor processor = provider.GetService<IProcessor>();
                    processor.Process(testPair);

                    IResultCollector collector = provider.GetService<IResultCollector>();

                    List<Result> results = collector.Results;
                    FailResult result = (FailResult)results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(errorKind, result.ValidationErrors[0].Kind);
                }
            }
        }
    }
}
