
using NJsonSchema;
using NSwag;
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

        private const string UnsanitizedContent = "application/json; charset=utf-8";
        private const string SanitizedContent = "application/json";

        [Fact]
        public void ValidateXml_ReturnsEmptyErrorListWithValidXml()
        {
            JsonSchema testJson = new JsonSchema();
            List<ValidationError> result = testJson.ValidateXml(AValidXml);
            Assert.Empty(result);
        }

        [Fact]
        public void ValidateXml_ReturnsErrorWithInvalidXml()
        {
            JsonSchema testJson = new JsonSchema();
            List<ValidationError> result = testJson.ValidateXml(AMalformedXml);
            Assert.Equal(ValidationErrorKind.InvalidXml, result.ElementAt(0).Kind);
        }

        [Fact]
        public void ToJsonText_ReturnsAValidJsonString()
        {
            string result = FutureJsonString.ToJsonText();
            Assert.Equal(ExpectedJsonString, result);
        }

        [Fact]
        public void SanitizeContentTypes_RemovesTrailingEncoding()
        {
            Dictionary<string, OpenApiMediaType> subject = new Dictionary<string, OpenApiMediaType>();
            subject.Add(UnsanitizedContent, new OpenApiMediaType());

            Dictionary<string, OpenApiMediaType> result = subject.SanitizeContentTypes();

            Assert.True(result.ContainsKey(SanitizedContent));
        }

    }
}
