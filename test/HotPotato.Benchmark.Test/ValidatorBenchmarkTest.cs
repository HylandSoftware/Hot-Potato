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
	//xUnit thinks that the PerfSetup should be a theory, so the warning needs to be disabled
	#pragma warning disable xUnit1013
	public class ValidatorBenchmarkTest
	{
		private Counter _counter;
		private IValidationStrategy validationStrategy;

		public ValidatorBenchmarkTest(ITestOutputHelper output)
		{
			ResultCollector resultCollector = new ResultCollector();

			string specPath = Path.Combine(Environment.CurrentDirectory, "PerfSpec.yaml");
			//stub this out on SpecificationProvider
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
		/// Ensure that we can validate at least 10,000 times per second.
		/// </summary>
		[NBenchFact]
		[PerfBenchmark(
			Description = "Ensure validation doesn't take too long",
			NumberOfIterations = 3,
			RunTimeMilliseconds = 1000,
			RunMode = RunMode.Throughput,
			TestMode = TestMode.Test)]
		[CounterThroughputAssertion("Iterations", MustBe.GreaterThan, 10000)]
		public void ValidationStrategy_Validate_Benchmark()
		{
			validationStrategy.Validate();
			_counter.Increment();
		}
	}
}
