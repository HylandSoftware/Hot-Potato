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
using System.Threading.Tasks;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class SpecStatCodeValTest
	{
		[SkippableTheory]
		[ClassData(typeof(StatusCodeNoContentTestData))]
		public async Task StatCodeVal_CreatesValidResultWithNullContent(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = null;
				var testResponse = await testRespMsg.ToClientResponseAsync();

				using (HotPotatoRequest testRequest = new HotPotatoRequest(reqMethod, new Uri(endpointURI)))
				using (HttpPair testPair = new HttpPair(testRequest, testResponse))
				{
					ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
					OpenApiDocument swagDoc = specPro.GetSpecDocument();

					IProcessor processor = provider.GetService<IProcessor>();
					processor.Process(testPair);

					IResultCollector collector = provider.GetService<IResultCollector>();

					List<Result> results = collector.Results;
					Result result = results.ElementAt(0);

					Assert.True(result.State == State.Pass, result.ToString());
				}
			}
		}

		[SkippableTheory]
		[ClassData(typeof(StatusCodeNoContentTestData))]
		public async Task StatCodeVal_CreatesValidResultWithEmptyContent(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = new StringContent("", Encoding.UTF8, "application/json");
				var testResponse = await testRespMsg.ToClientResponseAsync();

				using (HotPotatoRequest testRequest = new HotPotatoRequest(reqMethod, new Uri(endpointURI)))
				using (HttpPair testPair = new HttpPair(testRequest, testResponse))
				{
					ISpecificationProvider specPro = provider.GetService<ISpecificationProvider>();
					OpenApiDocument swagDoc = specPro.GetSpecDocument();

					IProcessor processor = provider.GetService<IProcessor>();
					processor.Process(testPair);

					IResultCollector collector = provider.GetService<IResultCollector>();

					List<Result> results = collector.Results;
					Result result = results.ElementAt(0);

					Assert.True(result.State == State.Pass, result.ToString());
				}
			}
		}

		[SkippableTheory]
		[ClassData(typeof(StatusCodeNoContentTestData))]
		public async Task StatCodeVal_CreatesInvalidResultWithUnexpContent(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = new StringContent("{ 'perfectSquare': '4' }", Encoding.UTF8, "application/json");
				var testResponse = await testRespMsg.ToClientResponseAsync();

				using (HotPotatoRequest testRequest = new HotPotatoRequest(reqMethod, new Uri(endpointURI)))
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
					Assert.Equal(Reason.UnexpectedBody, result.Reasons.ElementAt(0));
				}
			}
		}
	}
}
