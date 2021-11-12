using Newtonsoft.Json.Linq;
using NJsonSchema;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
	internal static class JsonSchemaExtensions
	{
		public static List<ValidationError> ValidateUndefinedProperties(this JsonSchema schema, string bodyString)
		{
			JToken bodyToken = JToken.Parse(bodyString);
			List<ValidationError> validationErrors = new List<ValidationError>();

			if (bodyToken is JObject)
			{
				JEnumerable<JToken> childTokens = bodyToken.Children();
				var schemaProperties = schema?.ActualSchema?.ActualProperties;

				if (schemaProperties != null)
				{
					foreach (JToken childToken in childTokens)
					{
						if (!schemaProperties.ContainsKey(childToken.Path))
						{
							validationErrors.Add(new ValidationError($"Property not found in spec: {childToken.Path}", ValidationErrorKind.PropertyNotInSpec, childToken.Path, 0, 0));
						}
					}
				}
			}

			return validationErrors;
		}
	}
}
