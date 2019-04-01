
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
            SwaggerPathItem swagPath = new SwaggerPathItem();
            SwaggerOperation expected = Mock.Of<SwaggerOperation>();
            swagPath.Add("get", expected);

            MethodValidator subject = new MethodValidator(HttpMethod.Get);

            Assert.True(subject.Validate(swagPath));
            Assert.Equal(expected, subject.Result);
        }

        [Fact]
        public void MethodValidator_ReturnsFalseWithMissingMethod()
        {
            SwaggerPathItem swagPath = new SwaggerPathItem();
            swagPath.Add("TRACE", Mock.Of<SwaggerOperation>());

            MethodValidator subject = new MethodValidator(HttpMethod.Get);

            Assert.False(subject.Validate(swagPath));
        }
    }
}
