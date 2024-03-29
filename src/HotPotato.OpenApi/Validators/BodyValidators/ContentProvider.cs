using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
	internal static class ContentProvider
	{
		public static JsonSchema GetSchema(OpenApiResponse swagResp, string contentType)
		{
			JsonSchema specBody = null;
			//application/problem+json doesn't generate the Content dict used below
			if (contentType.Contains("problem"))
			{
				specBody = swagResp.ActualResponse.Schema;
			}
			//Conditional for matching schemas with multiple content-type returns
			else if (swagResp?.Content != null && swagResp.Content?.Count > 0)
			{
				Dictionary<string, OpenApiMediaType> contentSchemas = swagResp.Content.SanitizeContentTypes();
				if (contentSchemas.ContainsKey(contentType))
				{
					specBody = contentSchemas[contentType].Schema;
				}
			}
			return specBody;
		}

		public static ValidationError[] GenerateContentError(string contentType)
		{
			ValidationError contentError = new ValidationError($"Content-Type '{contentType}' and its corresponding schema is missing in spec",
				ValidationErrorKind.MissingContentType, "Content-Type", 0, 0);
			return new ValidationError[1]{ contentError };
		}
	}
}
