using HotPotato.OpenApi.Models;

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
            Errors = new ValidationError[] { };
        }

        public InvalidResult(Reason reason, ValidationError[] errors)
        {
            Valid = false;
            Reason = reason;
            Errors = errors;
        }
    }
}
