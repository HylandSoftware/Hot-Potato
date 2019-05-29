using static HotPotato.IntegrationTestMethods;
using HotPotato.OpenApi.Results;
using Microsoft.Extensions.DependencyInjection;
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
    public class SpecStatCodeValTest
    {
        [Theory]
        [ClassData(typeof(StatusCodeNoContentTestData))]
        public async void StatCodeVal_CreatesValidResultWithNullContent(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = null;
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
        [ClassData(typeof(StatusCodeNoContentTestData))]
        public async void StatCodeVal_CreatesValidResultWithEmptyContent(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = new StringContent("", Encoding.UTF8, "application/json");
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
        [ClassData(typeof(StatusCodeNoContentTestData))]
        public async void StatCodeVal_CreatesInvalidResultWithUnexpContent(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
        {
            string specPath = SpecPath(specSubPath, "specification.yaml");
            ServiceProvider provider = GetServiceProvider(specPath);

            using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
            {
                testRespMsg.Content = new StringContent("{ 'perfectSquare': '4' }", Encoding.UTF8, "application/json");
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
                    FailResult result = (FailResult)results.ElementAt(0);

                    Assert.Equal(State.Fail, result.State);
                    Assert.Equal(Reason.UnexpectedBody, result.Reason);
                }
            }
        }
    }
}
