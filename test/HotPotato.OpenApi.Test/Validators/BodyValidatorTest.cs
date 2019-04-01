
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

            Assert.True(subject.Validate(swagResp));
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(AnInvalidBody);

            Assert.False(subject.Validate(swagResp));
            Assert.Equal(Reason.InvalidBody, subject.FailReason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, subject.ErrorArr[0].Kind);
        }
    }
}
