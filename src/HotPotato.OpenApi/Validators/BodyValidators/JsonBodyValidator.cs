using HotPotato.OpenApi.Filters;
using HotPotato.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
	internal class JsonBodyValidator : BodyValidator
	{
		public JsonBodyValidator(string bodyString)
		{
			BodyString = bodyString;
		}

		public override IValidationResult Validate(JsonSchema schema)
		{
			JToken bodyToken = JToken.Parse(BodyString);
			JEnumerable<JToken> childTokens = bodyToken.Children();
			var schemaProperties = schema?.ActualSchema?.ActualProperties;

			ICollection<NJsonSchema.Validation.ValidationError> errors = schema.Validate(BodyString);
			List<ValidationError> errList = errors.ToValidationErrorList();

			if (bodyToken is JObject)
			{
				foreach (JToken childToken in childTokens)
				{
					if (!schemaProperties.ContainsKey(childToken.Path))
					{
						errList.Add(new ValidationError($"Property not found in spec: {childToken.Path}", ValidationErrorKind.PropertyNotInSpec, childToken.Path, 0, 0));
					}
				}
			}

			List<IValidationErrorFilter> filters = FilterFactory.CreateApplicableFilters(schema, BodyString);
			foreach (IValidationErrorFilter filter in filters)
			{
				filter.Filter(errList);
			}
			if (errList.Count == 0)
			{
				return new ValidResult();
			}
			else
			{
				ValidationError[] errorArr = errList.ToArray();
				return new InvalidResult(Reason.InvalidBody, errorArr);
			}
		}
	}
}
