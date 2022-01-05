using Newtonsoft.Json.Linq;
using NJsonSchema;
using System.Collections.Generic;
using System.Linq;

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
					//responses should always come back with lowercase properties, but I still wanted to add this and the childToken.Path.ToLower() just in case
					List<string> schemaPropertyKeys = schemaProperties.Keys.Select(x => x.ToLower()).ToList();
					foreach (JToken childToken in childTokens)
					{
						if (!schemaPropertyKeys.Contains(childToken.Path.ToLower()))
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
