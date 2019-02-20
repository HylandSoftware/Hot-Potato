using System;

namespace HotPotato.Validators
{
    public static class ValidationErrorExtensions
    {
        public static ValidationErrorKind ToErrorKind(this string @this)
        {
            return (ValidationErrorKind)Enum.Parse(typeof(ValidationErrorKind), @this);
        }

    }
}
