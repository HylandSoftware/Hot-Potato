using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class ContentValidatorTest
	{
		private const string AValidBody = "{'foo': '1'}";

		[Fact]
		public void ContentValidator_ReturnsNullWithValidBodyandValidSchema()
		{
			JsonSchema schema = JsonSchema.CreateAnySchema();
			ContentValidator subject = new ContentValidator(AValidBody, new HttpContentType("application/json"));

			IValidationResult result = subject.Validate(schema);

			Assert.Null(result);
		}

		[Fact]
		public void ContentValidator_ReturnsInvalidWithMissingBody()
		{
			JsonSchema schema = JsonSchema.CreateAnySchema();
			ContentValidator subject = new ContentValidator("", new HttpContentType("application/json"));

			InvalidResult result = (InvalidResult)subject.Validate(schema);

			Assert.False(result.Valid);
			Assert.Equal(Reason.MissingBody, result.Reason);
		}

		[Fact]
		public void ContentValidator_ReturnsInvalidWithUnexpectedBody()
		{
			JsonSchema schema = null;
			ContentValidator subject = new ContentValidator(AValidBody, new HttpContentType("application/json"));

			InvalidResult result = (InvalidResult)subject.Validate(schema);

			Assert.False(result.Valid);
			Assert.Equal(Reason.MissingContentType, result.Reason);
		}

		[Fact]
		public void ContentValidator_ReturnsValidWithExpectedEmptyBody()
		{
			JsonSchema schema = null;
			ContentValidator subject = new ContentValidator(string.Empty, new HttpContentType("application/json"));

			IValidationResult result = subject.Validate(schema);

			Assert.True(result.Valid);
		}
	}
}
