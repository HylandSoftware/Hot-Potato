
using HotPotato.OpenApi.Models;
using NJsonSchema;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class TextBodyValidator : BodyValidator
    {
        public TextBodyValidator(string bodyString)
        {
            BodyString = bodyString;
        }

        public override IValidationResult Validate(JsonSchema4 schema)
        {
            BodyString = BodyString.ToJsonText();

            ICollection<NJsonSchema.Validation.ValidationError> errors = schema.Validate(BodyString);
            if (errors == null || errors.Count == 0)
            {
                return new ValidResult();
            }
            else
            {
                List<ValidationError> errList = errors.ToValidationErrorList();
                ValidationError[] errorArr = errList.ToArray();
                return new InvalidResult(Reason.InvalidBody, errorArr);
            }
        }
    }
}
