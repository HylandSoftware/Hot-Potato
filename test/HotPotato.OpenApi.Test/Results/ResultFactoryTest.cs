using HotPotato.Core.Http;
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

        private const string AValidKey = "A-Header-Key";
        private const string AValidValue = "A-Valid-Value";

        private const string AValidCustomHeaderKey = "X-HP-Custom-Header-Key";
        private const string AValidCustomHeaderValue = "Custom-Header-Value";

        [Fact]
        public void CanICreateAPassResult()
        {
            var subject = ResultFactory.PassResult(Path, Method, PassStatusCode, new HttpHeaders());

            Assert.NotNull(subject);
            Assert.Equal(Path, subject.Path);
            Assert.Equal(Method, subject.Method);
            Assert.Equal(PassStatusCode, subject.StatusCode);
            Assert.Equal(PassState, subject.State);
        }

        [Fact]
        public void CanICreateAPassResultWithCustomHeaders()
        {
            HttpHeaders customHeaders = new HttpHeaders();
            customHeaders.Add(AValidCustomHeaderKey, AValidCustomHeaderValue);

            PassResultWithCustomHeaders subject = (PassResultWithCustomHeaders)ResultFactory.PassResult(Path, Method, PassStatusCode, customHeaders);

            Assert.True(subject.Custom.ContainsKey(AValidCustomHeaderKey));
            Assert.Equal(AValidCustomHeaderValue, subject.Custom[AValidCustomHeaderKey].ElementAt(0));
        }

        [Fact]
        public void CanICreateAFailResult()
        {
            var err = new ValidationError("Error", ValidationErrorKind.Unknown, "Property", 5, 10);
            var validationErrors = new ValidationError[] { err };

            HttpHeaders customHeaders = new HttpHeaders();

            FailResult subject = (FailResult)ResultFactory.FailResult(Path, Method, FailStatusCode, new Reason[1] { FailReason }, customHeaders, validationErrors);

            Assert.NotNull(subject);
            Assert.Equal(Path, subject.Path);
            Assert.Equal(Method, subject.Method);
            Assert.Equal(FailStatusCode, subject.StatusCode);
            Assert.Equal(FailState, subject.State);
            Assert.Equal(FailReason, subject.Reasons.ElementAt(0));
            Assert.Equal(validationErrors, subject.ValidationErrors);
        }

        [Fact]
        public void CanICreateAFailResultWithCustomHeaders()
        {
            HttpHeaders customHeaders = new HttpHeaders();
            customHeaders.Add(AValidKey, AValidValue);

            var err = new ValidationError("Error", ValidationErrorKind.Unknown, "Property", 5, 10);
            var validationErrors = new ValidationError[] { err };

            FailResultWithCustomHeaders subject = (FailResultWithCustomHeaders)ResultFactory.FailResult(Path, Method, FailStatusCode, new Reason[1] { FailReason }, customHeaders, validationErrors);

            Assert.True(subject.Custom.ContainsKey(AValidKey));
            Assert.Equal(AValidValue, subject.Custom[AValidKey].ElementAt(0));
        }
    }
}
