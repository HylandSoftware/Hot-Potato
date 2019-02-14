using HotPotato.Results;
using NJsonSchema;
using Xunit;

namespace HotPotato.Validators
{
    public class BodyValidatorTest
    {
        private const string AValidBody = "{'foo': '1'}";
        private const string AnInvalidBody = "{'foo': 'abc'}";
        private const string AValidSchema = @"{'type': 'integer'}";

        [Fact]
        public void Validate_ValidBody()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            BodyValidator subject = new BodyValidator(schema);

            Result result = subject.Validate(AValidBody);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<BodyValidResult>(result);
        }

        [Fact]
        public void Validate_InvalidBody()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            BodyValidator subject = new BodyValidator(schema);

            Result result = subject.Validate(AnInvalidBody);

            Assert.NotNull(result);
            Assert.Equal("IntegerExpected", result.Reasons[0].Kind);
            Assert.IsAssignableFrom<BodyInvalidResult>(result);
        }
    }
}
