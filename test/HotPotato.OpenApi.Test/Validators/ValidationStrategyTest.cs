using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using System;
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

		private readonly Reason[] InvalidBodyReason = new Reason[] { Reason.InvalidBody };
		private readonly Reason[] InvalidHeaderReason = new Reason[] { Reason.InvalidHeaders };
		private readonly Reason[] InvalidBodyandHeaderReasons = new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders };

		private readonly Reason[] InvalidBodyMissingHeaderReason = new Reason[] { Reason.InvalidBody, Reason.MissingHeaders };
		private readonly Reason[] MissingBodyInvalidHeaderReason = new Reason[] { Reason.MissingBody, Reason.InvalidHeaders };
		private readonly Reason[] MissingBodyAndHeaderReason = new Reason[] { Reason.MissingBody, Reason.MissingHeaders };

		private readonly Reason[] MissingPathReason = new Reason[] { Reason.MissingPath };

		private readonly ValidationError[] BodyValidationError = new ValidationError[1] {
			new ValidationError("FirstBodyError", ValidationErrorKind.NumberTooBig, "", 0, 0) };

		private readonly ValidationError[] HeaderValidationError = new ValidationError[2] {
			new ValidationError("FirstHeaderError", ValidationErrorKind.PropertyRequired, "", 0, 0),
			new ValidationError("SecondHeaderError", ValidationErrorKind.TimeSpanExpected, "", 0, 0)};

		[Fact]
		public void AddValidationResult_CallsPassOnlyOnce()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt, null));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			ValidResult passingBodyResult = new ValidResult();
			ValidResult passingHeaderResult = new ValidResult();

			subject.AddValidationResult(passingBodyResult, passingHeaderResult);

			mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt, null), Times.Once());
		}

		[Fact]
		public void AddValidationResult_DoesntCallPassWithInvalidBodyAndValidHeaders()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, InvalidBodyReason, null, BodyValidationError));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody, BodyValidationError);
			ValidResult passingHeaderResult = new ValidResult();

			subject.AddValidationResult(failingBodyResult, passingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, InvalidBodyReason, null, BodyValidationError), Times.Once());
			mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt, new HttpHeaders()), Times.Never());
		}

		[Fact]
		public void AddValidationResult_DoesntCallPassWithInvalidHeadersAndValidBody()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, InvalidHeaderReason, null, HeaderValidationError));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			ValidResult passingBodyResult = new ValidResult();
			InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders, HeaderValidationError);

			subject.AddValidationResult(passingBodyResult, failingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, InvalidHeaderReason, null, HeaderValidationError), Times.Once());
			mockResultCollector.Verify(x => x.Pass(AValidPath, AValidMethodString, AValidStatusCodeInt, new HttpHeaders()), Times.Never());
		}

		[Fact]
		public void AddValidationResult_CallsFailOnceWithBothInvalidBodyandHeaders()
		{
			ValidationError[] expected = BodyValidationError.Concat(HeaderValidationError).ToArray();

			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt,
				InvalidBodyandHeaderReasons, null, expected));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody, BodyValidationError);
			InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders, HeaderValidationError);

			subject.AddValidationResult(failingBodyResult, failingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt,
				InvalidBodyandHeaderReasons, null, expected), Times.Once());
		}

		[Fact]
		public void AddValidationResult_CallsFailOnceWithBothInvalidBodyandHeadersAndNullErrors()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt,
				new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders }, null, Array.Empty<ValidationError>()));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody);
			InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders);

			subject.AddValidationResult(failingBodyResult, failingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt,
				new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders }, null, Array.Empty<ValidationError>()), Times.Once());
		}

		[Fact]
		public void AddValidationResult_DoesNotThrowExceptionWithMissingHeadersAndInvalidBodyWithError()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, InvalidBodyMissingHeaderReason, null, BodyValidationError));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			InvalidResult failingBodyResult = new InvalidResult(Reason.InvalidBody, BodyValidationError);
			InvalidResult failingHeaderResult = new InvalidResult(Reason.MissingHeaders);

			subject.AddValidationResult(failingBodyResult, failingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, InvalidBodyMissingHeaderReason, null, BodyValidationError), Times.Once());
		}

		[Fact]
		public void AddValidationResult_DoesNotThrowExceptionWithMissingBodyAndInvalidHeadersWithError()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, MissingBodyInvalidHeaderReason, null, HeaderValidationError));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			InvalidResult failingBodyResult = new InvalidResult(Reason.MissingBody);
			InvalidResult failingHeaderResult = new InvalidResult(Reason.InvalidHeaders, HeaderValidationError);

			subject.AddValidationResult(failingBodyResult, failingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, MissingBodyInvalidHeaderReason, null, HeaderValidationError), Times.Once());
		}

		[Fact]
		public void AddValidationResult_DoesNotThrowExceptionWithMissingBodyAndMissingHeaders()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, MissingBodyAndHeaderReason, null, Array.Empty<ValidationError>()));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object);

			InvalidResult failingBodyResult = new InvalidResult(Reason.MissingBody);
			InvalidResult failingHeaderResult = new InvalidResult(Reason.MissingHeaders);

			subject.AddValidationResult(failingBodyResult, failingHeaderResult);

			mockResultCollector.Verify(x => x.Fail(AValidPath, AValidMethodString, AValidStatusCodeInt, MissingBodyAndHeaderReason, null, Array.Empty<ValidationError>()), Times.Once());
		}

		[Fact]
		public void Validate_CallsFailWithMissingData()
		{
			Mock<IResultCollector> mockResultCollector = new Mock<IResultCollector>();
			mockResultCollector.Setup(x => x.Fail("", AValidMethodString, AValidStatusCodeInt, MissingPathReason, null, Array.Empty<ValidationError>()));

			ValidationStrategy subject = SetUpValidationStrategy(mockResultCollector.Object, "");
			subject.Validate();

			mockResultCollector.Verify(x => x.Fail("", AValidMethodString, AValidStatusCodeInt, MissingPathReason, null, Array.Empty<ValidationError>()), Times.Once());
		}

		private ValidationStrategy SetUpValidationStrategy(IResultCollector resColl, string path = AValidPath)
		{
			ValidationStrategy valStrat = new ValidationStrategy(resColl, Mock.Of<ISpecificationProvider>(), new HttpContentType("application/json"));
			valStrat.PathValidator = new PathValidator(path);
			valStrat.MethodValidator = new MethodValidator(AValidMethod);
			valStrat.StatusCodeValidator = new StatusCodeValidator(AValidStatusCode, "");
			return valStrat;
		}
	}
}
