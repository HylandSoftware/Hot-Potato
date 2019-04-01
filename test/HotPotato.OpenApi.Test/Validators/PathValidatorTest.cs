
using Moq;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class PathValidatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void PathValidator_GeneratesPathItem()
        {
            SwaggerDocument swagDoc = new SwaggerDocument();
            SwaggerPathItem expected = Mock.Of<SwaggerPathItem>();
            swagDoc.Paths.Add("/workflow/life-cycles", expected);

            PathValidator subject = new PathValidator(AValidEndpoint);

            Assert.True(subject.Validate(swagDoc));
            Assert.Equal(expected, subject.Result);
        }
        [Fact]
        public void PathValidator_ReturnsFailWithMissingPath()
        {
            SwaggerDocument swagDoc = new SwaggerDocument();
            swagDoc.Paths.Add("/deficiencies/deficiencies", Mock.Of<SwaggerPathItem>());

            PathValidator subject = new PathValidator(AValidEndpoint);

            Assert.False(subject.Validate(swagDoc));
        }

        [Fact]
        public void PathValidator_ReturnsFailWithNullPath()
        {
            SwaggerDocument swagDoc = new SwaggerDocument();
            swagDoc.Paths.Add("/deficiencies/deficiencies", Mock.Of<SwaggerPathItem>());

            PathValidator subject = new PathValidator(null);

            Assert.False(subject.Validate(swagDoc));
        }
    }
}
