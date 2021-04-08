
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
            OpenApiOperation swagOp = new OpenApiOperation();
            OpenApiResponse expected = Mock.Of<OpenApiResponse>();
            swagOp.Responses.Add("200", expected);

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.OK, "");

            Assert.True(subject.Validate(swagOp));
            Assert.Equal(expected, subject.Result);
        }
        [Fact]
        public void StatCodeValidator_ReturnsTrueWithNoContentExpected()
        {
            OpenApiOperation swagOp = new OpenApiOperation();
            OpenApiResponse expected = Mock.Of<OpenApiResponse>();
            swagOp.Responses.Add("204", expected);

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.NoContent, "");

            Assert.True(subject.Validate(swagOp));
            Assert.Equal(expected, subject.Result);
        }
        [Fact]
        public void StatCodeValidator_ReturnsFalseWithMissingStatCode()
        {
            OpenApiOperation swagOp = new OpenApiOperation();
            swagOp.Responses.Add("400", Mock.Of<OpenApiResponse>());

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.OK, "");

            Assert.False(subject.Validate(swagOp));
            Assert.Equal(Reason.MissingStatusCode, subject.FailReason);
        }

        [Theory]
        [InlineData("204", HttpStatusCode.NoContent)]
        [InlineData("205", HttpStatusCode.ResetContent)]
        [InlineData("304", HttpStatusCode.NotModified)]
        public void StatCodeValidator_ReturnsFalseWithUnexpectedBody(string NoContentStatusCode, HttpStatusCode StatusCode)
        {
            OpenApiOperation swagOp = new OpenApiOperation();
            swagOp.Responses.Add(NoContentStatusCode, Mock.Of<OpenApiResponse>());

            StatusCodeValidator subject = new StatusCodeValidator(StatusCode, "{'perfectSquare': '4'}");

            Assert.False(subject.Validate(swagOp));
            Assert.Equal(Reason.UnexpectedBody, subject.FailReason);
        }

        [Fact]
        public void StatCodeValidator_ReturnsFalseWithNullOpenApiResponse()
        {
            OpenApiOperation swagOp = new OpenApiOperation();

            StatusCodeValidator subject = new StatusCodeValidator(HttpStatusCode.Ambiguous, "{'perfectSquare': '49'}");

            Assert.False(subject.Validate(swagOp));
            Assert.Equal(Reason.MissingStatusCode, subject.FailReason);
        }

        //HttpStatusCodes are guarded against null, so a StatCodeValidator_ReturnsFalseWithNullStatusCode test isn't possible
    }

}