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
	//xUnit thinks that the PerfSetup should be a theory, so the warning needs to be disabled
	#pragma warning disable xUnit1013
	public class MatcherBenchmarkTest
	{
		private Counter _counter;

		//consider scalable lengths for path strings
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
		/// Ensure that we can match paths at least 500,000 times per second (5ms).
		/// </summary>
		[NBenchFact]
		[PerfBenchmark(
			Description = "Ensure matching doesn't take too long",
			NumberOfIterations = 3,
			RunTimeMilliseconds = 1000,
			RunMode = RunMode.Throughput,
			TestMode = TestMode.Test)]
		[CounterThroughputAssertion("Iterations", MustBe.GreaterThan, 500000)]
		public void PathMatcher_Match()
		{
			PathMatcher.Match(AValidPath, ValidPaths);
			_counter.Increment();
		}
	}
}
