using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using Newtonsoft.Json;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class HeaderValidator
    {
        public HttpHeaders headers { get; }

        public HeaderValidator(HttpHeaders Headers)
        {
            headers = Headers;
        }

        public IValidationResult Validate(SwaggerResponse swagResp)
        {
            if (swagResp.Headers != null && swagResp.Headers.Count > 0)
            {
                foreach (var item in swagResp.Headers)
                {
                    string headerKey = item.Key;
                    if (headers == null || !headers.ContainsKey(headerKey))
                    {
                        return new InvalidResult(Reason.MissingHeaders);
                    }
                    else
                    {
                        List<string> headerValues = headers[headerKey];
                        foreach (string value in headerValues)
                        {
                            // HACK - Need to convert to JSON because that's how NJsonSchema likes it.
                            string jValue = JsonConvert.SerializeObject(value);
                            ICollection<NJsonSchema.Validation.ValidationError> errors = item.Value.Validate(jValue);
                            if (errors != null && errors.Count != 0)
                            {
                                List<ValidationError> errList = errors.ToValidationErrorList();
                                ValidationError[] errorArr = errList.ToArray();
                                return new InvalidResult(Reason.InvalidHeaders, errorArr);
                            }

                        }
                    }

                }
            }
            return new ValidResult();
        }
    }
}
