using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Filters
{
	public interface IValidationErrorFilter
	{
		void Filter(IList<ValidationError> validationErrors);
	}
}
