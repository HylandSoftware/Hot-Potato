using HotPotato.OpenApi.Models;
using System;

namespace HotPotato.OpenApi.Validators
{
    public class InvalidResult : IValidationResult
    {
        public bool Valid { get; }
        public ValidationError[] Errors { get; }
        public Reason Reason { get; }

        public InvalidResult(Reason reason)
        {
            Valid = false;
            Reason = reason;
			Errors = Array.Empty<ValidationError>();
        }

        public InvalidResult(Reason reason, ValidationError[] errors)
        {
            Valid = false;
            Reason = reason;
            Errors = errors;
        }
    }
}
