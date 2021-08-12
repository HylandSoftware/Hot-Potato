using NJsonSchema;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Filters
{
	public class FilterFactoryTest
	{
		private const string AValidBody = "{'foo': '1'}";
		private const string AValidNullableBody = "{'foo': null}";


		[Fact]
		public void CreateApplicableFilters_CreatesNullableFilterWithApplicableBody()
		{
			JsonSchema schema = JsonSchema.CreateAnySchema();
			List<IValidationErrorFilter> errList = FilterFactory.CreateApplicableFilters(schema, AValidNullableBody);

			Assert.True(errList.ElementAt(0) is NullableValidationErrorFilter);
		}

		[Fact]
		public void CreateApplicableFilters_CreatesEmptyListWithStandardBody()
		{
			JsonSchema schema = JsonSchema.CreateAnySchema();
            
			List<IValidationErrorFilter> errList = FilterFactory.CreateApplicableFilters(schema, AValidBody);

			Assert.Empty(errList);
		}

	}
}
