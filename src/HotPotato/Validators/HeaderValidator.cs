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
    public class HeaderValidator
    {
        public ICollection<Result> Validate(HttpHeaders headers, SwaggerHeaders schema)
        {
            _ = headers ?? throw new ArgumentNullException(nameof(headers));
            _ = schema ?? throw new ArgumentNullException(nameof(schema));

            List<Result> results = new List<Result>();
            foreach (var item in schema)
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

                }            }
            return results;
        }
    }
}
