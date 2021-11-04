using HotPotato.OpenApi.Filters;
using HotPotato.OpenApi.Models;
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
			ICollection<NJsonSchema.Validation.ValidationError> errors = schema.Validate(BodyString);
			List<ValidationError> errList = errors.ToValidationErrorList();

			errList.AddRange(schema.ValidateUndefinedProperties(BodyString));

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
