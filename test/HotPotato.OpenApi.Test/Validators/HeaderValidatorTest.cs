
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class HeaderValidatorTest
    {
        private const string AValidHeaderKey = "X-Header-Key";
        private const string AValidHeaderValue = "value";
        private const string AValidSchema = @"{'type': 'integer'}";
        private const string AnInvalidValue = "invalidValue";

        [Fact]
        public void HeaderValidator_ReturnsFalseWithKeyNotFound()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.Headers.Add(AValidHeaderKey, new JsonSchema4());

            Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
            HeaderValidator subject = new HeaderValidator(headers);

            Assert.False(subject.Validate(swagResp));
            Assert.Equal(Reason.MissingHeaders, subject.FailReason);
        }

        [Fact]
        public void HeaderValidator_ReturnsTrueWithValidSchema()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.Headers.Add(AValidHeaderKey, JsonSchema4.CreateAnySchema());

            Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
            headers.Add(AValidHeaderKey, AValidHeaderValue);
            HeaderValidator subject = new HeaderValidator(headers);

            Assert.True(subject.Validate(swagResp));
        }

        [Fact]
        public void HeaderValidator_ReturnsFalseWithInvalidSchema()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.Headers.Add(AValidHeaderKey, JsonSchema4.FromJsonAsync(AValidSchema).Result);

            Core.Http.HttpHeaders headers = new Core.Http.HttpHeaders();
            headers.Add(AValidHeaderKey, AnInvalidValue);
            HeaderValidator subject = new HeaderValidator(headers);

            Assert.False(subject.Validate(swagResp));
            Assert.Equal(Reason.InvalidHeaders, subject.FailReason);
        }
    }
}
