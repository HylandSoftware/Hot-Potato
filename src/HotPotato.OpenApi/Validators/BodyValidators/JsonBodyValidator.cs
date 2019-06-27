using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
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

        public override IValidationResult Validate(JsonSchema4 schema)
        {
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
