
using HotPotato.OpenApi.Models;
using NJsonSchema;
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
            JsonBodyValidator subject = new JsonBodyValidator(AValidBody);

            IValidationResult result = subject.Validate(schema);

            Assert.True(result.Valid);
        }

        [Fact]
        public void JsonBodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            JsonBodyValidator subject = new JsonBodyValidator(AnInvalidBody);

            InvalidResult result = (InvalidResult)subject.Validate(schema);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, result.Errors[0].Kind);
        }

        //the cases for null body and null schema will now be addressed by the ContentValidator
    }
}
