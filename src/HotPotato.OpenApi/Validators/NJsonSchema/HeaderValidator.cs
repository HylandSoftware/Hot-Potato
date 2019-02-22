using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using Newtonsoft.Json;
using NSwag;
using System;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    public class HeaderValidator : IHeaderValidator
    {
        private readonly SwaggerHeaders swaggerHeaders;

        public HeaderValidator(SwaggerHeaders schema)
        {
            _ = schema ?? throw new ArgumentNullException(nameof(schema));

            this.swaggerHeaders = schema;
        }

        public ICollection<Result> Validate(HttpHeaders headers)
        {
            _ = headers ?? throw new ArgumentNullException(nameof(headers));

            List<Result> results = new List<Result>();
            foreach (var item in this.swaggerHeaders)
            {
                string headerKey = item.Key;
                if (!headers.ContainsKey(headerKey))
                {
                    results.Add(ResultFactory.HeaderNotFoundResult(headerKey));
                }
                else
                {
                    List<string> headerValues = headers[headerKey];
                    foreach (string value in headerValues)
                    {
                        // HACK - Need to convert to JSON because that's how NJsonSchema likes it.
                        string jValue = JsonConvert.SerializeObject(value);
                        ICollection<NJsonSchema.Validation.ValidationError> errors = item.Value.Validate(jValue);
                        if (errors == null || errors.Count == 0)
                        {
                            results.Add(ResultFactory.HeaderValidResult(headerKey, value));
                        }
                        else
                        {
                            List<ValidationError> errList = errors.ToValidationErrorList();
                            results.Add(ResultFactory.HeaderInvalidResult(headerKey, value, errList));
                        }
                    }

                }
            }
            return results;
        }
    }
}
