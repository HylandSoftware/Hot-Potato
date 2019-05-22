
using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class TextBodyValidator : BodyValidator
    {
        public TextBodyValidator(string bodyString, HttpContentType contentType)
        {
            BodyString = bodyString;
            ContentType = contentType;
        }

        public override IValidationResult Validate(SwaggerResponse swagResp)
        {
            JsonSchema4 specBody = GetSchema(swagResp);

            if (specBody == null)
            {
                return new InvalidResult(Reason.MissingSpecBody);
            }
            else if (string.IsNullOrWhiteSpace(BodyString))
            {
                return new InvalidResult(Reason.MissingBody);
            }

            BodyString = BodyString.ToJsonText();

            ICollection<NJsonSchema.Validation.ValidationError> errors = specBody.Validate(BodyString);
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
