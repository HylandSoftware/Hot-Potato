
using HotPotato.OpenApi.Models;
using NJsonSchema;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class XmlBodyValidatorTest
	{
		private const string AValidBody = @"<LGNotification><MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";
		private const string AnInvalidBody = @"<MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";

		[Fact]
		public void XmlBodyValidator_ReturnsTrueWithValid()
		{
			JsonSchema schema = JsonSchema.CreateAnySchema();
			XmlBodyValidator subject = new XmlBodyValidator(AValidBody);

			IValidationResult result = subject.Validate(schema);

			Assert.True(result.Valid);
		}

		[Fact]
		public void XmlBodyValidator_ReturnsFalseWithInvalid()
		{
			JsonSchema schema = JsonSchema.CreateAnySchema();
			XmlBodyValidator subject = new XmlBodyValidator(AnInvalidBody);

			InvalidResult result = (InvalidResult)subject.Validate(schema);

			Assert.False(result.Valid);
			Assert.Equal(Reason.InvalidBody, result.Reason);
			Assert.Equal(ValidationErrorKind.InvalidXml, result.Errors[0].Kind);
		}
	}
}
