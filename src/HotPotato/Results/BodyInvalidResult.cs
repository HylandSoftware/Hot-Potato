using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class BodyInvalidResult : InvalidResult
    {
        public override List<ValidationError> Reasons { get; }

        public override string Message { get; }

        public BodyInvalidResult(string content, List<ValidationError> reasons)
        {
            Message = Messages.BodyInvalid(content);
            Reasons = reasons;
        }
    }
}
