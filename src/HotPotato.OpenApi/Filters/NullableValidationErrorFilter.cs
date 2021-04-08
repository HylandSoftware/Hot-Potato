using HotPotato.OpenApi.Validators;
using Newtonsoft.Json;
using NJsonSchema;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Filters
{
	public class NullableValidationErrorFilter : IValidationErrorFilter
	{
		private readonly JsonSchema schema;
		private readonly string body;

		public NullableValidationErrorFilter(JsonSchema Schema, string Body)
		{
			schema = Schema;
			body = Body;
		}

		public void Filter(IList<ValidationError> validationErrors)
		{
			//need to make a copy to iterate through while removing false errors from the original referenced error colelction
			List<ValidationError> errorCopy = new List<ValidationError>(validationErrors);

			var properties = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(body);

			foreach (ValidationError error in errorCopy)
			{
				//this should never be null when testing with a live spec, but it occurred when unit testing with an unassociated spec and response body
				if (error.Property != null)
				{
					bool IsNullable = schema.ActualSchema?.Properties[error.Property]?.ActualSchema?.IsNullableRaw ?? false;
					if (IsNullable && properties[error.Property] == null)
					{
						validationErrors.Remove(error);
					}
				}

			}
		}
	}
}
