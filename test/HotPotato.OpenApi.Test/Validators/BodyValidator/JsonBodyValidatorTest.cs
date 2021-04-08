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

        private const string AValidNullableBody = "{'foo': null}";
        //nullable in yaml converts to x-nullable in json
        private const string AValidNullableSchema = @"{'properties':{'foo':{'type':'integer','x-nullable':true}}}";

        [Fact]
        public void JsonBodyValidator_ReturnsTrueWithValid()
        {
            JsonSchema schema = JsonSchema.CreateAnySchema();
            JsonBodyValidator subject = new JsonBodyValidator(AValidBody);

            IValidationResult result = subject.Validate(schema);

            Assert.True(result.Valid);
        }

        [Fact]
        public void JsonBodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema schema = JsonSchema.FromJsonAsync(AValidSchema).Result;
            JsonBodyValidator subject = new JsonBodyValidator(AnInvalidBody);

            InvalidResult result = (InvalidResult)subject.Validate(schema);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, result.Errors[0].Kind);
        }

        //the cases for null body and null schema will now be addressed by the ContentValidator

        [Fact]
        public void JsonBodyValidator_ReturnsTrueWithValidNullable()
        {
            JsonSchema schema = JsonSchema.FromJsonAsync(AValidNullableSchema).Result;
            JsonBodyValidator subject = new JsonBodyValidator(AValidNullableBody);

            IValidationResult result = subject.Validate(schema);

            Assert.True(result.Valid);
        }

        [Fact]
        public void JsonBodyValidator_ReturnsFalseWithUnexpectedNullable()
        {
            JsonSchema schema = JsonSchema.FromJsonAsync(AValidSchema).Result;
            JsonBodyValidator subject = new JsonBodyValidator(AValidNullableBody);

            InvalidResult result = (InvalidResult)subject.Validate(schema);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, result.Errors[0].Kind);
        }
    }
}
