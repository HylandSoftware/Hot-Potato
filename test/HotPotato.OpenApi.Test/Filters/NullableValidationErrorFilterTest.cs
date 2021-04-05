using HotPotato.OpenApi.Validators;
using NJsonSchema;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Filters
{
	public class NullableValidationErrorFilterTest
	{
		private const string AValidBody = "{'foo': '1'}";
		private const string AnInvalidBody = "{'foo': 'abc'}";
		private const string AValidSchema = @"{'type': 'integer'}";

		private const string AValidNullableBody = "{'foo': null}";
		//nullable in yaml converts to x-nullable in json
		private const string AValidNullableSchema = @"{'properties':{'foo':{'type':'integer','x-nullable':true}}}";

		[Fact]
		public void NullableValidationErrorFilter_Filter_RemovesFalseNullErrors()
		{
			JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidNullableSchema).Result;

			ICollection<NJsonSchema.Validation.ValidationError> errors = schema.Validate(AValidNullableBody);
			List<ValidationError> errList = errors.ToValidationErrorList();

			NullableValidationErrorFilter filter = new NullableValidationErrorFilter(schema, AValidNullableBody);

			filter.Filter(errList);

			Assert.Empty(errList);
		}

		[Fact]
		public void NullableValidationErrorFilter_Filter_DoesNotRemoveTrueErrors()
		{
			JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;

			ICollection<NJsonSchema.Validation.ValidationError> errors = schema.Validate(AValidNullableBody);
			List<ValidationError> errList = errors.ToValidationErrorList();

			NullableValidationErrorFilter filter = new NullableValidationErrorFilter(schema, AValidNullableBody);

			filter.Filter(errList);

			Assert.Single(errList);
			Assert.Contains("IntegerExpected", errList.ElementAt(0).Message);

		}
	}
}
