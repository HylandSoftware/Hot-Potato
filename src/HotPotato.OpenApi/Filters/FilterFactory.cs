using NJsonSchema;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Filters
{
	public static class FilterFactory
	{
		public static List<IValidationErrorFilter> CreateApplicableFilters(JsonSchema schema, string body)
		{
			List<IValidationErrorFilter> filters = new List<IValidationErrorFilter>();
			if (body.Contains("null"))
			{
				filters.Add(new NullableValidationErrorFilter(schema, body));
			}
			//new filters with applicable conditions can be added here
			return filters;
		}
	}
}
