
using NJsonSchema;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class BodyValidatorExtensionsTest
    {
        private const string AValidXml = @"<BaseResponse><Code>48</Code><Message>Deleted with a valid xml</Message></BaseResponse>";
        private const string AMalformedXml = @"<BaseResponse><Code>48</Code><Message>Deleted with a malformed xml</Message>";

        private const string FutureJsonString = "JsonString";
        private const string ExpectedJsonString = "\"JsonString\"";

        [Fact]
        public void ValidateXml_ReturnsEmptyErrorListWithValidXml()
        {
            JsonSchema4 testJson = new JsonSchema4();
            List<ValidationError> result = testJson.ValidateXml(AValidXml);
            Assert.Empty(result);
        }

        [Fact]
        public void ValidateXml_ReturnsErrorWithInvalidXml()
        {
            JsonSchema4 testJson = new JsonSchema4();
            List<ValidationError> result = testJson.ValidateXml(AMalformedXml);
            Assert.Equal(ValidationErrorKind.InvalidXml, result.ElementAt(0).Kind);
        }

        [Fact]
        public void ToJsonText_ReturnsAValidJsonString()
        {
            string result = FutureJsonString.ToJsonText();
            Assert.Equal(ExpectedJsonString, result);
        }
    }
}
