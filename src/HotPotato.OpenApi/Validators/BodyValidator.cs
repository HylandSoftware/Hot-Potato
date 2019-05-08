
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class BodyValidator
    {
        public string BodyString { get; }

        public BodyValidator(string bodyString)
        {
            if (string.IsNullOrWhiteSpace(bodyString))
            {
                BodyString = "";
            }
            else
            {
                BodyString = bodyString;
            }
        }

        public IValidationResult Validate(SwaggerResponse swagResp)
        {
            if (swagResp.ActualResponse == null || swagResp.ActualResponse.Schema == null)
            {
                return new InvalidResult(Reason.MissingSpecBody);
            }
            else if(BodyString == "")
            {
                return new InvalidResult(Reason.MissingBody);
            }
            JsonSchema4 specBody = swagResp.ActualResponse.Schema;
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
