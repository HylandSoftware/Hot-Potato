using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationStrategyTest
    {
        private const string AValidPath = "/path";
        private const string AValidMethodString = "trace";
        private const int AValidStatusCodeInt = 451;
        private const HttpStatusCode AValidStatusCode = HttpStatusCode.UnavailableForLegalReasons;

        private readonly HttpMethod AValidMethod = HttpMethod.Trace;

        private readonly ValidationError[] FirstValidationError = new ValidationError[1] {
            new ValidationError("FirstBodyError", ValidationErrorKind.NumberTooBig, "", 0, 0) };

        private readonly ValidationError[] SecondValidationError = new ValidationError[2] {
            new ValidationError("FirstHeaderError", ValidationErrorKind.PropertyRequired, "", 0, 0),
            new ValidationError("SecondHeaderError", ValidationErrorKind.TimeSpanExpected, "", 0, 0)};

        [Fact]
        public void AddValidationResult_CallsPassOnlyOnce()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            ValidResult passingBodyResult = new ValidResult();
            ValidResult passingHeaderResult = new ValidResult();

            subject.AddValidationResult(passingBodyResult, passingHeaderResult);

            mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt), Times.Once());
        }

        [Fact]
        public void AddValidationResult_DoesntCallPassWithInvalidBodyAndValidHeaders()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidBody, FirstValidationError));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody, FirstValidationError);
            ValidResult passingHeaderResult = new ValidResult();

            subject.AddValidationResult(failingBodyResult, passingHeaderResult);

            mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, new Reason[] { Reason.InvalidBody }, FirstValidationError), Times.Once());
            mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt), Times.Never());
        }

        [Fact]
        public void AddValidationResult_DoesntCallPassWithInvalidHeadersAndValidBody()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidHeaders, SecondValidationError));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            ValidResult passingBodyResult = new ValidResult();
            InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders, SecondValidationError);

            subject.AddValidationResult(passingBodyResult, failingHeaderResult);

            mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, new Reason[] { Reason.InvalidHeaders }, SecondValidationError), Times.Once());
            mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt), Times.Never());
        }

        [Fact]
        public void AddValidationResult_CallsFailOnceWithBothInvalidBodyandHeaders()
        {
            ValidationError[] expected = FirstValidationError.Concat(SecondValidationError).ToArray();

            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, 
                new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders }, expected));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody, FirstValidationError);
            InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders, SecondValidationError);

            subject.AddValidationResult(failingBodyResult, failingHeaderResult);

            mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, 
                new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders }, expected), Times.Once());
        }

        [Fact]
        public void AddValidationResult_CallsFailOnceWithBothInvalidBodyandHeadersAndNullErrors()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt,
                new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders }, null));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody);
            InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders);

            subject.AddValidationResult(failingBodyResult, failingHeaderResult);

            mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt,
                new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders }, null), Times.Once());
        }

        [Fact]
        public void Validate_CallsFailWithMissingData()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail("", AValidMethodString, AValidStatusCodeInt, Reason.MissingPath));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object, "");
            subject.Validate();

            mockResultCollector.Verify(x => x.Fail("", AValidMethodString, AValidStatusCodeInt, new Reason[] { Reason.MissingPath }), Times.Once());
        }

        private ValidationStrategy SetUpValidationStrategy(IResultCollector resColl, string path = AValidPath)
        {
            ValidationStrategy valStrat = new ValidationStrategy(resColl, Mock.Of<ISpecificationProvider>());
            valStrat.PathValidator = new PathValidator(path);
            valStrat.MethodValidator = new MethodValidator(AValidMethod);
            valStrat.StatusCodeValidator = new StatusCodeValidator(AValidStatusCode, "");
            return valStrat;
        }
    }
}
