using HotPotato.OpenApi.Matchers;
using NBench;
using Pro.NBench.xUnit.XunitExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace HotPotato.Benchmark.Test
{
	public class MatcherBenchmarkTest
	{
		private Counter _counter;

		private const string AValidPath = "/foo/bar";
		private readonly List<string> ValidPaths = new List<string>
			{
				"/a/b/c",
				"/a/b",
				"/foo/bar",
				"/d/c"
			};

		public MatcherBenchmarkTest(ITestOutputHelper output)
		{
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
			PathMatcher.Match(AValidPath, ValidPaths);
			_counter.Increment();
		}
	}
}
