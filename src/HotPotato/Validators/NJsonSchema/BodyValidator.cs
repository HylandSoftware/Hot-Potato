using HotPotato.Results;
using NJsonSchema;
using NJsonSchema.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators.NJsonSchema
{
    public class BodyValidator : IBodyValidator
    {
        private readonly JsonSchema4 schema;
        
        public BodyValidator(JsonSchema4 schema)
        {
            _ = schema ?? throw new ArgumentNullException(nameof(schema));
            this.schema = schema;
        }

        public Result Validate(string content)
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
