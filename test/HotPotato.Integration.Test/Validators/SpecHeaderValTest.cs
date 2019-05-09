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
    public class SpecHeaderValTest
    {
        [Theory]
        [ClassData(typeof(SpecHeaderTestData))]
        public async void HeaderValidator_CreatesValidResultWithoutMatchingCase(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            string bodyString = JsonConvert.SerializeObject(bodyJson);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                //Made HotPot's HttpHeaders' dict constructor case insensitive for possible edge cases
                //The key is capital "Location" in the spec
                testRespMsg.Headers.Add("location", "http://api.docs.hyland.io/docs.html?url=http%3A%2F%2Fapi.docs.hyland.io%2Fdocument%2Fspecification.json");
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
                    Result result = results.ElementAt(1);

                    Assert.Equal(State.Pass, result.State);

                }
            }
        }

        [Theory]
        [ClassData(typeof(SpecHeaderTestData))]
        public async void HeaderValidator_CreatesMissingHeaderResult(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
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
                    Result result = results.ElementAt(1);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.MissingHeaders, result.Reason);

                }
            }
        }
    }
}
