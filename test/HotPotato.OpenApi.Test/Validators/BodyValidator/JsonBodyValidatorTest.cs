using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class JsonBodyValidatorTest
    {
        private const string AValidBody = "{'foo': '1'}";
        private const string AnInvalidBody = "{'foo': 'abc'}";
        private const string AValidSchema = @"{'type': 'integer'}";

        [Fact]
        public void JsonBodyValidator_ReturnsTrueWithValid()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            JsonBodyValidator subject = new JsonBodyValidator(AValidBody, new HttpContentType("application/json"));
            IValidationResult result = subject.Validate(swagResp);

            Assert.True(result.Valid);
        }

        [Fact]
        public void JsonBodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            JsonBodyValidator subject = new JsonBodyValidator(AnInvalidBody, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, result.Errors[0].Kind);
        }

        [Fact]
        public void JsonBodyValidator_ReturnsFalseWithNullBody()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            JsonBodyValidator subject = new JsonBodyValidator(null, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingBody, result.Reason);
        }

        [Fact]
        public void JsonBodyValidator_ReturnsFalseWithNullSchema()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = null;

            BodyValidator subject = new JsonBodyValidator(AValidBody, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingSpecBody, result.Reason);
        }
    }
}
