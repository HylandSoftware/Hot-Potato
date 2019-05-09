
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class BodyValidatorTest
    {
        private const string AValidBody = "{'foo': '1'}";
        private const string AnInvalidBody = "{'foo': 'abc'}";
        private const string AValidSchema = @"{'type': 'integer'}";

        [Fact]
        public void BodyValidator_ReturnsTrueWithValid()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(AValidBody);
            IValidationResult result = subject.Validate(swagResp);

            Assert.True(result.Valid);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(AnInvalidBody);
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, result.Errors[0].Kind);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithNullBody()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(null);
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingBody, result.Reason);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithNullSchema()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = null;

            BodyValidator subject = new BodyValidator(AValidBody);
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingSpecBody, result.Reason);
        }
    }
}
