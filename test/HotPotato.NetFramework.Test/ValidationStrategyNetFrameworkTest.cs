using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.OpenApi.Validators
{
	internal class ValidationStrategyNetFrameworkTest
	{
		private const string AValidPath = "/path";
		private const string AValidMethodString = "trace";
		private const int AValidStatusCodeInt = 300;
		private const HttpStatusCode AValidStatusCode = HttpStatusCode.MultipleChoices;

		private readonly HttpMethod AValidMethod = HttpMethod.Trace;

		private readonly Reason[] InvalidBodyandHeaderReasons = new Reason[] { Reason.InvalidBody, Reason.InvalidHeaders };

		private readonly ValidationError[] BodyValidationError = new ValidationError[1] {
			new ValidationError("FirstBodyError", ValidationErrorKind.NumberTooBig, "", 0, 0) };

		private readonly ValidationError[] HeaderValidationError = new ValidationError[2] {
			new ValidationError("FirstHeaderError", ValidationErrorKind.PropertyRequired, "", 0, 0),
			new ValidationError("SecondHeaderError", ValidationErrorKind.TimeSpanExpected, "", 0, 0)};

		[Test]
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

		[Test]
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
