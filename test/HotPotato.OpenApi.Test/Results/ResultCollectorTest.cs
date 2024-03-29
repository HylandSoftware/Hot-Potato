using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Results
{
	public class ResultCollectorTest
	{
		private const string Path = "/endpoint";
		private const string Get = "GET";
		private const int OkStatusCode = 200;
		private const int NotFoundStatusCode = 404;
		private const string Uri = "http://localhost/endpoint";

		private readonly Reason[] inputReason = new Reason[] { Reason.Unknown };
		private readonly List<Reason> expectedReason = new List<Reason>() { Reason.Unknown };

		[Fact]
		public void CanIAddAPassResultToResultsList()
		{
			PassResult expected = new PassResult(Path, Get, OkStatusCode);

			ResultCollector subject = new ResultCollector();

			subject.Pass(Path, Get, OkStatusCode, null);

			Assert.NotEmpty(subject.Results);
			Assert.Single(subject.Results);
			Assert.Equal(expected.Path, subject.Results[0].Path);
			Assert.Equal(expected.Method, subject.Results[0].Method);
			Assert.Equal(expected.StatusCode, subject.Results[0].StatusCode);
			Assert.Equal(expected.State, subject.Results[0].State);
		}

		[Fact]
		public void CanIAddAFailResultToResultsList()
		{
			var err = new ValidationError("Error", ValidationErrorKind.Unknown, "Property", 5, 10);
			var validationErrors = new List<ValidationError> { err };

			FailResult expected = new FailResult(Path, Get, NotFoundStatusCode, expectedReason, validationErrors);

			ResultCollector subject = new ResultCollector();

			subject.Fail(Path, Get, NotFoundStatusCode, inputReason, null, validationErrors.ToArray());

			Assert.NotEmpty(subject.Results);
			Assert.Single(subject.Results);

			FailResult result = (FailResult)subject.Results[0];

			Assert.Equal(expected.Path, result.Path);
			Assert.Equal(expected.Method, result.Method);
			Assert.Equal(expected.StatusCode, result.StatusCode);
			Assert.Equal(expected.State, result.State);
			Assert.Equal(expected.Reasons.ElementAt(0), result.Reasons.ElementAt(0));
			Assert.Equal(expected.ValidationErrors, result.ValidationErrors);
		}

		[Fact]
		public void ResultCollector_OverallResult_ReflectsFail()
		{
			ResultCollector subject = new ResultCollector();

			subject.Pass(Path, Get, OkStatusCode, null);
			subject.Fail(Path, Get, NotFoundStatusCode, inputReason, null);

			Assert.Equal(State.Fail, subject.OverallResult);
		}

		[Fact]
		public void ResultCollector_OverallResult_ReflectsPass()
		{
			ResultCollector subject = new ResultCollector();

			subject.Pass(Path, Get, OkStatusCode, null);

			Assert.Equal(State.Pass, subject.OverallResult);
		}
	}
}
