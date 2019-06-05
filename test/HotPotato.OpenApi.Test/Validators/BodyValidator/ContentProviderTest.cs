
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class ContentProviderTest
    {
        string AValidJsonContentType = "application/json";
        string AValidProblemContentType = "application/problem+json";
        string AValidXmlContentType = "application/xml";

        [Fact]
        public void GetSchema_ReturnsBodyWithValidContent()
        {
            JsonSchema4 expected = JsonSchema4.CreateAnySchema();

            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.Schema = expected;

            JsonSchema4 result = ContentProvider.GetSchema(swagResp, AValidJsonContentType);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetSchema_ReturnsBodyWithValidProblemContent()
        {
            SwaggerResponse swagResp = new SwaggerResponse();

            JsonSchema4 expected = JsonSchema4.CreateAnySchema();
            OpenApiMediaType mediaType = new OpenApiMediaType();
            mediaType.Schema = expected;

            swagResp.Content.Add(AValidProblemContentType, mediaType);

            JsonSchema4 result = ContentProvider.GetSchema(swagResp, AValidProblemContentType);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetSchema_ReturnsNullWithNullContent()
        {
            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.Schema = null;

            JsonSchema4 result = ContentProvider.GetSchema(swagResp, AValidJsonContentType);

            Assert.Null(result);
        }

        [Fact]
        public void GetSchema_ReturnsNullWithMissingContent()
        {
            JsonSchema4 expected = JsonSchema4.CreateAnySchema();

            SwaggerResponse swagResp = new SwaggerResponse();
            swagResp.Schema = expected;

            JsonSchema4 result = ContentProvider.GetSchema(swagResp, AValidXmlContentType);

            Assert.Null(result);
        }

        [Fact]
        public void GenerateContentError_CreatesMessageWithCorrectData()
        {
            ValidationError[] results = ContentProvider.GenerateContentError(AValidJsonContentType);
            ValidationError result = results[0];

            Assert.Contains(AValidJsonContentType, result.Message);
            Assert.Equal(ValidationErrorKind.MissingContent, result.Kind);

        }
    }
}
