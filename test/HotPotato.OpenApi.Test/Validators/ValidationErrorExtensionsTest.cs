using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class ValidationErrorExtensionsTest
	{
		string AValidErrorKind = "Unknown";

		[Fact]
		public void ToErrorKind_CanParse()
		{
			ValidationErrorKind result = AValidErrorKind.ToErrorKind();
			Assert.Equal(ValidationErrorKind.Unknown, result);
		}

		[Fact]
		public void ToValidationErrorList_Can_Handle_Null()
		{
			ICollection<NJsonSchema.Validation.ValidationError> subject = null;

			List<ValidationError> result = subject.ToValidationErrorList();

			Assert.Empty(result);
		}

		[Fact]
		public void ToValidationErrorList_Can_Convert_Correctly()
		{
			NJsonSchema.Validation.ValidationError unknownError = new NJsonSchema.Validation.ValidationError(0, string.Empty, string.Empty, null, null);
			ICollection<NJsonSchema.Validation.ValidationError> subject = new List<NJsonSchema.Validation.ValidationError>()
			{
				unknownError
			};

			List<ValidationError> result = subject.ToValidationErrorList();

			Assert.Single(result);
			Assert.Equal(ValidationErrorKind.Unknown, result.ElementAt(0).Kind);
		}
	}
}
