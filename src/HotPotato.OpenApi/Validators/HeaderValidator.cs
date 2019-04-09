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
        public Reason FailReason { get; private set; }
        public ValidationError[] ErrorArr { get; private set; }

        public HeaderValidator(HttpHeaders Headers)
        {
            headers = Headers;
        }

        public bool Validate(SwaggerResponse swagResp)
        {
            if (swagResp.Headers != null && swagResp.Headers.Count > 0)
            {
                foreach (var item in swagResp.Headers)
                {
                    string headerKey = item.Key;
                    if (headers == null || !headers.ContainsKey(headerKey))
                    {
                        FailReason = Reason.MissingHeaders;
                        return false;
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
                                ErrorArr = errList.ToArray();
                                FailReason = Reason.InvalidHeaders;
                                return false;
                            }

                        }
                    }

                }
            }
            return true;
        }
    }
}
