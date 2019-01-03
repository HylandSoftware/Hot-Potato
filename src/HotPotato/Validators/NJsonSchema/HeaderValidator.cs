using HotPotato.Http;
using HotPotato.Results;
using Newtonsoft.Json;
using NJsonSchema.Validation;
using NSwag;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators
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
                        ICollection<ValidationError> result = item.Value.Validate(jValue);
                        if (result == null || result.Count == 0)
                        {
                            results.Add(ResultFactory.HeaderValidResult(headerKey, value));
                        }
                        else
                        {
                            results.Add(ResultFactory.HeaderInvalidResult(headerKey, value, result));
                        }
                    }

                }
            }
            return results;
        }
    }
}
