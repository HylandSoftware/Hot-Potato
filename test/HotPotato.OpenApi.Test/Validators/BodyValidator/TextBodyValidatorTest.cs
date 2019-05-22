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
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = BodyValidatorFactory.Create(AValidBody, new HttpContentType("text/plain"));

            Assert.True(subject.Validate(swagResp).Valid);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithEmptyText()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            TextBodyValidator subject = new TextBodyValidator(AnInvalidBody, new HttpContentType("text/plain"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingBody, result.Reason);
        }
    }
}
