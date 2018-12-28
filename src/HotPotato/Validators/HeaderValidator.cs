using HotPotato.Http;
using HotPotato.Results;
using NJsonSchema.Validation;
using NSwag;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators
{
    public class HeaderValidator
    {
        public IEnumerable<Result> Validate(HttpHeaders headers, SwaggerHeaders schema)
        {
            _ = headers ?? throw new ArgumentNullException(nameof(headers));
            _ = schema ?? throw new ArgumentNullException(nameof(schema));

            foreach (var item in schema)
            {
                string headerKey = item.Key;
                if (!headers.ContainsKey(headerKey))
                {
                    yield return ResultFactory.HeaderNotFoundResult(headerKey);
                }
                List<string> headerValues = headers[headerKey];
                foreach (string value in headerValues)
                {
                    ICollection<ValidationError> result = item.Value.Validate(value);
                    if (result == null || result.Count == 0)
                    {
                        yield return ResultFactory.HeaderValidResult(headerKey, value);
                    }
                    else
                    {
                        yield return ResultFactory.HeaderInvalidResult(headerKey, value, result);
                    }
                }
            }
        }
    }
}
