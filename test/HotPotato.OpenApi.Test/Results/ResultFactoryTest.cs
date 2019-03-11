using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using Xunit;

namespace HotPotato.Results
{
    public class ResultFactoryTest
    {
        private const string Path = "/endpoint";
        private const string Method = "GET";
        private const int PassStatusCode = 200;
        private const int FailStatusCode = 404;
        private const State PassState = State.Pass;
        private const State FailState = State.Fail;
        private const Reason FailReason = Reason.Unknown;

        [Fact]
        public void CanICreateAPassResult()
        {
            var subject = ResultFactory.PassResult(Path, Method, PassStatusCode, PassState);

            Assert.NotNull(subject);
            Assert.Equal(Path, subject.Path);
            Assert.Equal(Method, subject.Method);
            Assert.Equal(PassStatusCode, subject.StatusCode);
            Assert.Equal(PassState, subject.State);
        }

        [Fact]
        public void CanICreateAFailResult()
        {
            var err = new ValidationError("Error", ValidationErrorKind.Unknown, "Property", 5, 10);
            var validationErrors = new ValidationError[] { err };

            var subject = ResultFactory.FailResult(Path, Method, FailStatusCode, FailState, FailReason, validationErrors);

            Assert.NotNull(subject);
            Assert.Equal(Path, subject.Path);
            Assert.Equal(Method, subject.Method);
            Assert.Equal(FailStatusCode, subject.StatusCode);
            Assert.Equal(FailState, subject.State);
            Assert.Equal(FailReason, subject.Reason);
            Assert.Equal(validationErrors, subject.ValidationErrors);
        }
    }
}
