
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
            OpenApiDocument swagDoc = new OpenApiDocument();
            OpenApiPathItem expected = Mock.Of<OpenApiPathItem>();
            swagDoc.Paths.Add("/workflow/life-cycles", expected);

            PathValidator subject = new PathValidator(AValidEndpoint);

            Assert.True(subject.Validate(swagDoc));
            Assert.Equal(expected, subject.Result);
        }
        [Fact]
        public void PathValidator_ReturnsFalseWithMissingPath()
        {
            OpenApiDocument swagDoc = new OpenApiDocument();
            swagDoc.Paths.Add("/deficiencies/deficiencies", Mock.Of<OpenApiPathItem>());

            PathValidator subject = new PathValidator(AValidEndpoint);

            Assert.False(subject.Validate(swagDoc));
        }

        [Fact]
        public void PathValidator_ReturnsFalseWithNullPath()
        {
            OpenApiDocument swagDoc = new OpenApiDocument();
            swagDoc.Paths.Add("/deficiencies/deficiencies", Mock.Of<OpenApiPathItem>());

            PathValidator subject = new PathValidator(null);

            Assert.False(subject.Validate(swagDoc));
        }

        [Fact]
        public void PathValidator_ReturnsFalseWithNullSwaggerPath()
        {
            OpenApiDocument swagDoc = new OpenApiDocument();

            PathValidator subject = new PathValidator(AValidEndpoint);

            Assert.False(subject.Validate(swagDoc));
        }
    }
}
