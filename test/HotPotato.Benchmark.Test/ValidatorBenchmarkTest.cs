using HotPotato.OpenApi.Matchers;
using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using HotPotato.OpenApi.SpecificationProvider;
using HotPotato.Test.Api.Models;
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
		private IValidationStrategy validationStrategy;

		public ValidatorBenchmarkTest(ITestOutputHelper output)
		{
			ResultCollector resultCollector = new ResultCollector();

			string specPath = Path.Combine(Environment.CurrentDirectory, "PerfSpec.yaml");
			OpenApiDocument spec = OpenApiYamlDocument.FromFileAsync(specPath).Result;
			Mock<ISpecificationProvider> mockSpecPro = new Mock<ISpecificationProvider>();
			mockSpecPro.Setup(x => x.GetSpecDocument()).Returns(spec);

			validationStrategy = new ValidationBuilder(resultCollector, mockSpecPro.Object)
				.WithPath("/order")
				.WithMethod(HttpMethod.Post)
				.WithStatusCode(HttpStatusCode.Created)
				.WithBody("{\"id\":5,\"price\":10.0,\"items\":[]}", new HttpContentType("application/json"))
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
		/// Ensure that we can validate at least 30,000 times per second.
		/// </summary>
		[NBenchFact]
		[PerfBenchmark(
			Description = "Ensure validation doesn't take too long",
			NumberOfIterations = 3,
			RunTimeMilliseconds = 1000,
			RunMode = RunMode.Throughput,
			TestMode = TestMode.Test)]
		[CounterThroughputAssertion("Iterations", MustBe.GreaterThan, 30000)]
		public void ValidationStrategy_Validate()
		{
			validationStrategy.Validate();
			_counter.Increment();
		}
	}
}
