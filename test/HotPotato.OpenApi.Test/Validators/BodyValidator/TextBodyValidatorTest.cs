using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class TextBodyValidatorTest
    {
        private const string AValidBody = "Content";
        private const string AnInvalidBody = "  ";

        [Fact]
        public void BodyValidator_ReturnsTrueWithValidText()
        {
            SwaggerResponse swagResp = new SwaggerResponse();

            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            OpenApiMediaType mediaType = new OpenApiMediaType();
            mediaType.Schema = schema;

            swagResp.Content.Add("text/plain", mediaType);

            BodyValidator subject = BodyValidatorFactory.Create(AValidBody, new HttpContentType("text/plain"));

            Assert.True(subject.Validate(swagResp).Valid);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithEmptyText()
        {
            SwaggerResponse swagResp = new SwaggerResponse();

            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            OpenApiMediaType mediaType = new OpenApiMediaType();
            mediaType.Schema = schema;

            swagResp.Content.Add("text/plain", mediaType);

            TextBodyValidator subject = new TextBodyValidator(AnInvalidBody, new HttpContentType("text/plain"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingBody, result.Reason);
        }
    }
}
