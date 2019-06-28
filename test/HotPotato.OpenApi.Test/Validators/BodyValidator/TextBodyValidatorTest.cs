
using NJsonSchema;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class TextBodyValidatorTest
    {
        private const string AValidBody = "Content";
        private const string AnInvalidBody = "  ";

        [Fact]
        public void TextBodyValidator_ReturnsTrueWithValidText()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            TextBodyValidator subject = new TextBodyValidator(AValidBody);

            IValidationResult result = subject.Validate(schema);

            Assert.True(result.Valid);
        }

        //the cases of null and empty text will now be taken care of by the ContentValidator
    }
}
