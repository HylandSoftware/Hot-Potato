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
	public class SpecHeaderValTest
	{
		private const string LocationHeader = "location";
		private const string AValidLocationUri = "http://api.docs.hyland.io/";
		private const string AnInvalidLocationUri = @"this isn't a uri";

		[SkippableTheory]
		[ClassData(typeof(SpecHeaderTestData))]
		public async Task HeaderValidator_CreatesValidResultWithoutMatchingCase(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			string bodyString = JsonConvert.SerializeObject(bodyJson);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				//Made HotPot's HttpHeaders' dict constructor case insensitive for possible edge cases
				//The key is capital "Location" in the spec
				testRespMsg.Headers.Add(LocationHeader, AValidLocationUri);
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
					//The validation strategy should now only create one passing result for both body and header
					Result result = results.ElementAt(0);

					Assert.True(result.State == State.Pass, result.ToString());

				}
			}
		}


		[SkippableTheory]
		[ClassData(typeof(SpecHeaderTestData))]
		public async Task HeaderValidator_CreatesInvalidResultWithIncorrectFormat(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			string bodyString = JsonConvert.SerializeObject(bodyJson);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{
				testRespMsg.Headers.Add(LocationHeader, AnInvalidLocationUri);
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
					Assert.Equal(Reason.InvalidHeaders, result.Reasons.ElementAt(0));
					Assert.Equal(ValidationErrorKind.UriExpected, result.ValidationErrors.ElementAt(0).Kind);
				}
			}
		}

		[SkippableTheory]
		[ClassData(typeof(SpecHeaderTestData))]
		public async Task HeaderValidator_CreatesMissingHeaderResult(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, object bodyJson)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			string bodyString = JsonConvert.SerializeObject(bodyJson);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{

				testRespMsg.Content = new StringContent(bodyString, Encoding.UTF8, contentType);
				//no headers added

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
					Assert.Equal(Reason.MissingHeaders, result.Reasons.ElementAt(0));
				}
			}
		}

		[SkippableTheory]
		[ClassData(typeof(SpecHeaderWithExpectedNoContentTestData))]
		public async Task HeaderValidator_CreatesValidResultWithExpectedEmptyBody(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{

				testRespMsg.Headers.Add(LocationHeader, AValidLocationUri);
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
		[ClassData(typeof(SpecHeaderWithUnexpectedContentTestData))]
		public async Task HeaderValidator_CreatesMissingContentTypeResultWithUnexpectedBodyAndValidHeaders(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI, string contentType, string bodyString)
		{
			string specPath = SpecPath(specSubPath, "specification.yaml");
			Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

			ServiceProvider provider = GetServiceProvider(specPath);

			using (HttpResponseMessage testRespMsg = new HttpResponseMessage(statusCode))
			{

				testRespMsg.Headers.Add(LocationHeader, AValidLocationUri);
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
					Assert.Equal(Reason.MissingContentType, result.Reasons.ElementAt(0));
				}
			}
		}

		[SkippableTheory]
		[ClassData(typeof(SpecHeaderWithExpectedNoContentTestData))]
		public async Task HeaderValidator_CreatesOnlyMissingHeadersResultWithExpectedEmptyBody(string specSubPath, HttpMethod reqMethod, HttpStatusCode statusCode, string endpointURI)
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
					FailResult result = (FailResult)results.ElementAt(0);

					Assert.Equal(State.Fail, result.State);
					Assert.Equal(Reason.MissingHeaders, result.Reasons.ElementAt(0));
				}
			}
		}
	}
}
