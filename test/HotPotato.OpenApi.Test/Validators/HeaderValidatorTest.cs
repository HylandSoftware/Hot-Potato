
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class HeaderValidatorTest
	{
		private const string AValidHeaderKey = "X-Header-Key";
		private const string AValidHeaderValue = "value";
		private const string AValidEncodedHeaderValue = "dmFsdWU=";
		private const string AValidSchema = @"{'type': 'integer'}";
		private const string AnInvalidValue = "invalidValue";
		private const string NotAUriString = @"this isn't a uri";

		[Fact]
		public void HeaderValidator_ReturnsFalseWithMissingHeaders()
		{
			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, new JsonSchema());
			Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();

			HeaderValidator subject = new HeaderValidator(headers);
			InvalidResult result = (InvalidResult)subject.Validate(swagResp);

			Assert.False(result.Valid);
			Assert.Equal(Reason.MissingHeaders, result.Reason);
		}

		[Fact]
		public void HeaderValidator_ReturnsFalseWithNullHeaders()
		{
			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, new JsonSchema());

			HeaderValidator subject = new HeaderValidator(null);
			InvalidResult result = (InvalidResult)subject.Validate(swagResp);

			Assert.False(result.Valid);
			Assert.Equal(Reason.MissingHeaders, result.Reason);
		}

		[Fact]
		public void HeaderValidator_ReturnsFalseWithNullOpenApiResponse()
		{
			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, new JsonSchema());

			HeaderValidator subject = new HeaderValidator(null);
			InvalidResult result = (InvalidResult)subject.Validate(swagResp);

			Assert.False(result.Valid);
			Assert.Equal(Reason.MissingHeaders, result.Reason);
		}

		[Fact]
		public void HeaderValidator_ReturnsTrueWithValidSchema()
		{
			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, JsonSchema.CreateAnySchema());

			Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
			headers.Add(AValidHeaderKey, AValidHeaderValue);

			HeaderValidator subject = new HeaderValidator(headers);
			IValidationResult result = subject.Validate(swagResp);

			Assert.True(result.Valid);
		}

		[Fact]
		public void HeaderValidator_ReturnsFalseWithInvalidSchema()
		{
			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, JsonSchema.FromJsonAsync(AValidSchema).Result);

			Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
			headers.Add(AValidHeaderKey, AnInvalidValue);

			HeaderValidator subject = new HeaderValidator(headers);
			InvalidResult result = (InvalidResult)subject.Validate(swagResp);

			Assert.False(result.Valid);
			Assert.Equal(Reason.InvalidHeaders, result.Reason);
		}

		[Fact]
		public void HeaderValidator_ReturnsFalseWithIncorrectFormat()
		{
			JsonSchema uriSchema = new JsonSchema();
			uriSchema.Type = JsonObjectType.String;
			uriSchema.Format = JsonFormatStrings.Uri;

			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, uriSchema);

			Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
			headers.Add(AValidHeaderKey, NotAUriString);

			HeaderValidator subject = new HeaderValidator(headers);
			InvalidResult result = (InvalidResult)subject.Validate(swagResp);

			Assert.False(result.Valid);
			Assert.Equal(Reason.InvalidHeaders, result.Reason);
			Assert.Equal(ValidationErrorKind.UriExpected, result.Errors[0].Kind);
		}

		[Fact]
		public void HeaderValidator_ValidatesBase64Format()
		{
			JsonSchema byteSchema = new JsonSchema();
			byteSchema.Type = JsonObjectType.String;
			byteSchema.Format = JsonFormatStrings.Byte;

			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Headers.Add(AValidHeaderKey, byteSchema);

			Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
			headers.Add(AValidHeaderKey, AValidEncodedHeaderValue);

			HeaderValidator subject = new HeaderValidator(headers);
			IValidationResult result = subject.Validate(swagResp);

			Assert.True(result.Valid);
		}
	}
}
