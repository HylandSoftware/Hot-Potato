using HotPotato.Results;
using NJsonSchema;
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
            ICollection<NJsonSchema.Validation.ValidationError> errors = schema.Validate(content);
            if (errors == null || errors.Count == 0)
            {
                return ResultFactory.BodyValidResult(content);
            }
            else
            {
                List<ValidationError> hotPotList = new List<ValidationError>();
                foreach (NJsonSchema.Validation.ValidationError err in errors)
                {
                    hotPotList.Add(new ValidationError(err.ToString(), err.Kind.ToString().ToErrorKind(), err.Property, err.LineNumber, err.LinePosition));
                }
                return ResultFactory.BodyInvalidResult(content, hotPotList);
            }
        }
    }
}
