
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
			JsonSchema expected = JsonSchema.CreateAnySchema();

			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Schema = expected;

			JsonSchema result = ContentProvider.GetSchema(swagResp, AValidJsonContentType);

			Assert.Equal(expected, result);
		}

		[Fact]
		public void GetSchema_ReturnsBodyWithValidProblemContent()
		{
			OpenApiResponse swagResp = new OpenApiResponse();

			JsonSchema expected = JsonSchema.CreateAnySchema();
			OpenApiMediaType mediaType = new OpenApiMediaType();
			mediaType.Schema = expected;

			swagResp.Content.Add(AValidProblemContentType, mediaType);

			JsonSchema result = ContentProvider.GetSchema(swagResp, AValidProblemContentType);

			Assert.Equal(expected, result);
		}

		[Fact]
		public void GetSchema_ReturnsNullWithNullContent()
		{
			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Schema = null;

			JsonSchema result = ContentProvider.GetSchema(swagResp, AValidJsonContentType);

			Assert.Null(result);
		}

		[Fact]
		public void GetSchema_ReturnsNullWithMissingContent()
		{
			JsonSchema expected = JsonSchema.CreateAnySchema();

			OpenApiResponse swagResp = new OpenApiResponse();
			swagResp.Schema = expected;

			JsonSchema result = ContentProvider.GetSchema(swagResp, AValidXmlContentType);

			Assert.Null(result);
		}

		[Fact]
		public void GenerateContentError_CreatesMessageWithCorrectData()
		{
			ValidationError[] results = ContentProvider.GenerateContentError(AValidJsonContentType);
			ValidationError result = results[0];

			Assert.Contains(AValidJsonContentType, result.Message);
			Assert.Equal(ValidationErrorKind.MissingContentType, result.Kind);

		}
	}
}
