using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class XmlBodyValidatorTest
    {
        private const string AValidBody = @"<LGNotification><MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";
        private const string AnInvalidBody = @"<MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";

        [Fact]
        public void XmlBodyValidator_ReturnsTrueWithValid()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            XmlBodyValidator subject = new XmlBodyValidator(AValidBody, new HttpContentType("application/json"));
            IValidationResult result = subject.Validate(swagResp);

            Assert.True(result.Valid);
        }

        [Fact]
        public void XmlBodyValidator_ReturnsFalseWithInvalid()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.ActualResponse.Schema = schema;

            XmlBodyValidator subject = new XmlBodyValidator(AnInvalidBody, new HttpContentType("application/json"));
            InvalidResult result = (InvalidResult)subject.Validate(swagResp);

            Assert.False(result.Valid);
            Assert.Equal(Reason.InvalidBody, result.Reason);
            Assert.Equal(ValidationErrorKind.InvalidXml, result.Errors[0].Kind);
        }
    }
}
