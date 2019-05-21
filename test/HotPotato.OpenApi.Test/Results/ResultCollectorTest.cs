using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
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

        [Fact]
        public void CanIAddAPassResultToResultsList()
        {
            PassResult expected = new PassResult(Path, Get, OkStatusCode);

            ResultCollector subject = new ResultCollector();

            subject.Pass(Path, Get, OkStatusCode);

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

            FailResult expected = new FailResult(Path, Get, NotFoundStatusCode, Reason.Unknown, validationErrors);

            ResultCollector subject = new ResultCollector();

            subject.Fail(Path, Get, NotFoundStatusCode, Reason.Unknown, validationErrors.ToArray());

            Assert.NotEmpty(subject.Results);
            Assert.Single(subject.Results);

            FailResult result = (FailResult)subject.Results[0];

            Assert.Equal(expected.Path, result.Path);
            Assert.Equal(expected.Method, result.Method);
            Assert.Equal(expected.StatusCode, result.StatusCode);
            Assert.Equal(expected.State, result.State);
            Assert.Equal(expected.Reason, result.Reason);
            Assert.Equal(expected.ValidationErrors, result.ValidationErrors);
        }

        [Fact]
        public void ResultCollector_OverallResult_ReflectsFail()
        {
            ResultCollector subject = new ResultCollector();

            subject.Pass(Path, Get, OkStatusCode);
            subject.Fail(Path, Get, NotFoundStatusCode, Reason.Unknown, null);

            Assert.Equal(State.Fail, subject.OverallResult);
        }

        [Fact]
        public void ResultCollector_OverallResult_ReflectsPass()
        {
            ResultCollector subject = new ResultCollector();

            subject.Pass(Path, Get, OkStatusCode);

            Assert.Equal(State.Pass, subject.OverallResult);
        }
    }
}
