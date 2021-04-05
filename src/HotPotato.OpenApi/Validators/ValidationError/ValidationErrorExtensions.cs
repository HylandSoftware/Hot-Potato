using System;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    public static class ValidationErrorExtensions
    {
        public static ValidationErrorKind ToErrorKind(this string @this)
        {
            return (ValidationErrorKind)Enum.Parse(typeof(ValidationErrorKind), @this);
        }
        public static List<ValidationError> ToValidationErrorList(this ICollection<NJsonSchema.Validation.ValidationError> @this)
        {
            List<ValidationError> errList = new List<ValidationError>();
            if (@this != null)
			{
                foreach (NJsonSchema.Validation.ValidationError err in @this)
                {
                    errList.Add(new ValidationError(err.ToString(), err.Kind.ToString().ToErrorKind(), err.Property, err.LineNumber, err.LinePosition));
                }
            }
            return errList;
        }
    }
}
