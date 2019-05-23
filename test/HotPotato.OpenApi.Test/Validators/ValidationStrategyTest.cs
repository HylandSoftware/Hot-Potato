using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
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

        private HttpMethod AValidMethod = HttpMethod.Trace;
        private const HttpStatusCode AValidStatusCode = HttpStatusCode.UnavailableForLegalReasons;

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
            mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidBody, null));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody);
            ValidResult passingHeaderResult = new ValidResult();

            subject.AddValidationResult(failingBodyResult, passingHeaderResult);

            mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidBody, null), Times.Once());
            mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt), Times.Never());
        }

        [Fact]
        public void AddValidationResult_DoesntCallPassWithInvalidHeadersAndValidBody()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidHeaders, null));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

            ValidResult passingBodyResult = new ValidResult();
            InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders);

            subject.AddValidationResult(passingBodyResult, failingHeaderResult);

            mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidHeaders, null), Times.Once());
            mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt), Times.Never());
        }

        //TODO: Test for when the invalid body and header results are combined: AUTOTEST-344
        //[Fact]
        //public void AddValidationResult_CallsFailOnceWithBothInvalidBodyandHeaders()
        //{
        //    Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
        //    mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidHeaders, null));

        //    ValidationStrategy mockResultCollector = SetUpValidationStrategy(mockResultCollector.Object);

        //    InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody);
        //    InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders);

        //    mockResultCollector.AddValidationResult(failingBodyResult, failingHeaderResult);

        //    mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, Reason.InvalidBody, null), Times.Once());
        //}

        [Fact]
        public void Validate_CallsFailWithMissingData()
        {
            Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
            mockResultCollector.Setup(x => x.Fail("", AValidMethodString, AValidStatusCodeInt, Reason.MissingPath));

            ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object, "");
            subject.Validate();

            mockResultCollector.Verify(x => x.Fail("", AValidMethodString, AValidStatusCodeInt, Reason.MissingPath), Times.Once());
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
