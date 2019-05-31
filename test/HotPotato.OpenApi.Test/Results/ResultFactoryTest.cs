using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Results
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
            var subject = ResultFactory.PassResult(Path, Method, PassStatusCode);

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

            FailResult subject = (FailResult)ResultFactory.FailResult(Path, Method, FailStatusCode, new Reason[1] { FailReason }, validationErrors);

            Assert.NotNull(subject);
            Assert.Equal(Path, subject.Path);
            Assert.Equal(Method, subject.Method);
            Assert.Equal(FailStatusCode, subject.StatusCode);
            Assert.Equal(FailState, subject.State);
            Assert.Equal(FailReason, subject.Reasons.ElementAt(0));
            Assert.Equal(validationErrors, subject.ValidationErrors);
        }
    }
}
