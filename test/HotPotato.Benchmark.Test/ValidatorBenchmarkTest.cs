using HotPotato.OpenApi.Matchers;
using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using NBench;
using Newtonsoft.Json;
using NSwag;
using Pro.NBench.xUnit.XunitExtensions;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HotPotato.Benchmark.Test
{
	public class ValidatorBenchmarkTest
	{
		private Counter _counter;
		private OpenApiDocument spec;
		private OpenApiPathItem path;
		private OpenApiOperation method;
		private OpenApiResponse response;

		private const string AValidPath = "/foo/bar";
		private readonly List<string> ValidPaths = new List<string>
			{
				"/a/b/c",
				"/a/b",
				"/foo/bar",
				"/d/c"
			};

		public ValidatorBenchmarkTest(ITestOutputHelper output)
		{
			ResultCollector resultCollector = new ResultCollector();

			string specPath = Path.Combine(Environment.CurrentDirectory, "spec.yaml");
			spec = OpenApiYamlDocument.FromFileAsync(specPath).Result;

			Mock<SpecificationProvider> mockSpecPro = new Mock<SpecificationProvider>();
			mockSpecPro.Setup(x => x.GetSpecDocument()).Returns(spec);

			IValidationStrategy val = new ValidationBuilder(resultCollector, mockSpecPro.Object)
				.WithPath("/order")
				.WithMethod(HttpMethod.Post)
				.WithStatusCode(HttpStatusCode.Created)
				.WithBody("", new HttpContentType("application/json"))
				.Build();

			Trace.Listeners.Clear();
			Trace.Listeners.Add(new XunitTraceListener(output));
		}

		[PerfSetup]
		public void Setup(BenchmarkContext context)
		{
			_counter = context.GetCounter("Iterations");
		}

		/// <summary>
		/// Ensure that we can serialise at least 200 times per second (5ms).
		/// </summary>
		[NBenchFact]
		[PerfBenchmark(
			Description = "Ensure serialization doesn't take too long",
			NumberOfIterations = 3,
			RunTimeMilliseconds = 1000,
			RunMode = RunMode.Throughput,
			TestMode = TestMode.Test)]
		[CounterThroughputAssertion("Iterations", MustBe.GreaterThan, 200)]
		public void PathMatcher_Match()
		{

		}
	}
}
