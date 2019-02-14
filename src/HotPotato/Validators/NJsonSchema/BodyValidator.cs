using HotPotato.Results;
using NJsonSchema;
using NJsonSchema.Validation;
using System;
using System.Collections.Generic;

namespace HotPotato.Validators
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
                List<HotPotatoValidationError> hotPotList = new List<HotPotatoValidationError>();
                foreach (ValidationError err in errors)
                {
                    hotPotList.Add(new HotPotatoValidationError(err));
                }
                return ResultFactory.BodyInvalidResult(content, hotPotList);
            }
        }
    }
}
