using HotPotato.Core.Http;
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
        private const string AValidXmlBody = @"<LGNotification><MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";

        [Fact]
        public void BodyValidator_ReturnsTrueWithValid()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = BodyValidatorFactory.Create(AValidBody, new HttpContentType("application/json"));
            IValidationResult result = subject.Validate(swagResp);

            Assert.True(result.Valid);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = BodyValidatorFactory.Create(AnInvalidBody, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, result.Errors[0].Kind);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithNullBody()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = BodyValidatorFactory.Create(null, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingBody, result.Reason);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithNullSchema()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = null;

            BodyValidator subject = BodyValidatorFactory.Create(AValidBody, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.MissingSpecBody, result.Reason);
        }

        [Fact]
        public void BodyValidator_ReturnsTrueWithPlainText()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = BodyValidatorFactory.Create("Content", new HttpContentType("text/plain"));

            Assert.True(subject.Validate(swagResp).Valid);
        }

        [Fact]
        public void BodyValidator_ReturnsTrueWithXml()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = BodyValidatorFactory.Create(AValidXmlBody, new HttpContentType("application/xml"));

            Assert.True(subject.Validate(swagResp).Valid);
        }
    }
}
