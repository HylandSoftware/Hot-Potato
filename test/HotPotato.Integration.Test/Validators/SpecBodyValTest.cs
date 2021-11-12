
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
using System.Threading.Tasks;
using Xunit;

namespace HotPotato.OpenApi.Validators
{

	public class SpecBodyValTest
	{
		[SkippableTheory]
		[ClassData(typeof(SpecBodyValidTestData))]
		public async Task BodyValidator_CreatesValidResult(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			string bodyString = JsonConvert.SerializeObject(bodyJson);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
				//Content-Language header now needs to be set for workflow spec -
				//it also doubles up as a good test for external ref handling
				testRespMsg.Content.Headers.Add("Content-Language", "en-US");
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
		[ClassData(typeof(SpecBodyInvalidTestData))]
		public async Task BodyValidator_CreatesInvalidResult(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode,
			string endpointURI, string contentType, object bodyJson, ValidationErrorKind expectedKind1, ValidationErrorKind expectedKind2)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			string bodyString = JsonConvert.SerializeObject(bodyJson);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
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

					Assert.Equal(Reason.InvalidBody, result.Reasons.ElementAt(0));
					Assert.Equal(expectedKind1, result.ValidationErrors[0].Kind);
					if (result.ValidationErrors.Count > 1)
					{
						Assert.Equal(expectedKind2, result.ValidationErrors[1].Kind);
					}
				}
			}

		}

		[SkippableTheory]
		[ClassData(typeof(CustomSpecTestData))]
		public async Task BodyValidator_CreatesValidResultWithDiffTypes(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, string bodyString)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
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
		[ClassData(typeof(CustomSpecNegTestData))]
		public async Task BodyValidator_CreatesInvalidResultWithDiffTypes(string specSubPath, HttpMethod reqMethod,
			HttpStatusCode statusCode, string endpointURI, string contentType, string bodyString, ValidationErrorKind errorKind)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
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
					Assert.Equal(errorKind, result.ValidationErrors[0].Kind);
				}
			}
		}
	}
}
