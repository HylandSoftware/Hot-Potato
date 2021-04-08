
using Moq;
using NSwag;
using System.Net.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class MethodValidatorTest
    {
        [Fact]
        public void MethodValidator_GeneratesOperation()
        {
            OpenApiPathItem swagPath = new OpenApiPathItem();
            OpenApiOperation expected = Mock.Of<OpenApiOperation>();
            swagPath.Add("get", expected);

            MethodValidator subject = new MethodValidator(HttpMethod.Get);

            Assert.True(subject.Validate(swagPath));
            Assert.Equal(expected, subject.Result);
        }

        [Fact]
        public void MethodValidator_ReturnsFalseWithMissingMethod()
        {
            OpenApiPathItem swagPath = new OpenApiPathItem();
            swagPath.Add("trace", Mock.Of<OpenApiOperation>());

            MethodValidator subject = new MethodValidator(HttpMethod.Get);

            Assert.False(subject.Validate(swagPath));
        }

        [Fact]
        public void MethodValidator_ReturnsFalseWithNullMethod()
        {
            OpenApiPathItem swagPath = new OpenApiPathItem();

            swagPath.Add("trace", Mock.Of<OpenApiOperation>());

            MethodValidator subject = new MethodValidator(null);

            Assert.False(subject.Validate(swagPath));
        }

        [Fact]
        public void MethodValidator_ReturnsFalseWithNullSwaggerOp()
        {
            OpenApiPathItem swagPath = new OpenApiPathItem();

            MethodValidator subject = new MethodValidator(HttpMethod.Get);

            Assert.False(subject.Validate(swagPath));
        }

    }
}
