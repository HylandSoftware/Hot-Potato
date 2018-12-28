using HotPotato.Results;
using NJsonSchema;
using NJsonSchema.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators
{
    public class BodyValidator
    {
        public Result Validate(string content, JsonSchema4 schema)
        {
            ICollection<ValidationError> errors = schema.Validate(content);
            if (errors == null || errors.Count == 0)
            {
                return ResultFactory.BodyValidResult(content);
            }
            else
            {
                return ResultFactory.BodyInvalidResult(content, errors);
            }
        }
    }
}
