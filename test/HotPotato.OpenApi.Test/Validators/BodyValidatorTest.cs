
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

            BodyValidator subject = new BodyValidator(AValidBody, "application/json");

            Assert.True(subject.Validate(swagResp));
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(AnInvalidBody, "application/json");

            Assert.False(subject.Validate(swagResp));
            Assert.Equal(Reason.InvalidBody, subject.FailReason);
            Assert.Equal(ValidationErrorKind.IntegerExpected, subject.ErrorArr[0].Kind);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithNullBody()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(null, "application/json");

            Assert.False(subject.Validate(swagResp));
            Assert.Equal(Reason.MissingBody, subject.FailReason);
        }

        [Fact]
        public void BodyValidator_ReturnsFalseWithNullSchema()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = null;

            BodyValidator subject = new BodyValidator(AValidBody, "application/json");

            Assert.False(subject.Validate(swagResp));
            Assert.Equal(Reason.MissingSpecBody, subject.FailReason);
        }

        [Fact]
        public void BodyValidator_ReturnsTrueWithPlainText()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator("Content", "text/plain");

            Assert.True(subject.Validate(swagResp));
        }

        [Fact]
        public void BodyValidator_ReturnsTrueWithXml()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            BodyValidator subject = new BodyValidator(AValidXmlBody, "application/xml");

            Assert.True(subject.Validate(swagResp));
        }
    }
}
