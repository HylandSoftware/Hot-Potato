
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class BodyValidator
    {
        public string bodyString;
        public Reason FailReason;
        public ValidationError[] ErrorArr;

        public BodyValidator(string BodyString)
        {
            bodyString = BodyString;
        }

        public bool Validate(SwaggerResponse swagResp)
        {
            if (swagResp.ActualResponse == null || swagResp.ActualResponse.Schema == null)
            {
                FailReason = Reason.MissingSpecBody;
                return false;
            }
            else if(string.IsNullOrWhiteSpace(bodyString))
            {
                bodyString = "";
                FailReason = Reason.MissingBody;
                return false;
            }
            JsonSchema4 specBody = swagResp.ActualResponse.Schema;
            ICollection<NJsonSchema.Validation.ValidationError> errors = specBody.Validate(bodyString);
            if (errors == null || errors.Count == 0)
            {
                return true;
            }
            else
            {
                List<ValidationError> errList = errors.ToValidationErrorList();
                ErrorArr = errList.ToArray();
                FailReason = Reason.InvalidBody;
                return false;
            }
        }
    }
}
