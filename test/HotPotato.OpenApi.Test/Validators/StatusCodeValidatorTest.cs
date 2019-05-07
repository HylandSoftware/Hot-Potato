
using HotPotato.OpenApi.Models;
using Moq;
using NSwag;
using System.Net;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class StatusCodeValidatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void StatCodeValidator_GeneratesResponse()
        {
            SwaggerOperation swagOp = new SwaggerOperation();
            SwaggerResponse expected = Mock.Of<SwaggerResponse>();
            swagOp.Responses.Add("200", expected);

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.OK, "");

            Assert.True(subject.Validate(swagOp));
            Assert.Equal(expected, subject.Result);
        }
        [Fact]
        public void StatCodeValidator_ReturnsTrueWithNoContentExpected()
        {
            SwaggerOperation swagOp = new SwaggerOperation();
            SwaggerResponse expected = Mock.Of<SwaggerResponse>();
            swagOp.Responses.Add("204", expected);

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.NoContent, "");

            Assert.True(subject.Validate(swagOp));
            Assert.Equal(expected, subject.Result);
        }
        [Fact]
        public void StatCodeValidator_ReturnsFalseWithMissingStatCode()
        {
            SwaggerOperation swagOp = new SwaggerOperation();
            swagOp.Responses.Add("400", Mock.Of<SwaggerResponse>());

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.OK, "");

            Assert.False(subject.Validate(swagOp));
            Assert.Equal(Reason.MissingStatusCode, subject.FailReason);
        }
        [Fact]
        public void StatCodeValidator_ReturnsFalseWithUnexpectedBody()
        {
            SwaggerOperation swagOp = new SwaggerOperation();
            swagOp.Responses.Add("204", Mock.Of<SwaggerResponse>());

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.NoContent, "{'perfectSquare': '4'}");

            Assert.False(subject.Validate(swagOp));
            Assert.Equal(Reason.UnexpectedBody, subject.FailReason);
        }

        [Fact]
        public void StatCodeValidator_ReturnsFalseWithNullSwaggerResponse()
        {
            SwaggerOperation swagOp = new SwaggerOperation();

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.Ambiguous, "{'perfectSquare': '49'}");

            Assert.False(subject.Validate(swagOp));
            Assert.Equal(Reason.MissingStatusCode, subject.FailReason);
        }

        //HttpStatusCodes are guarded against null, so a StatCodeValidator_ReturnsFalseWithNullStatusCode test isn't possible
    }

}