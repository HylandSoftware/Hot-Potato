using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public class BodyInvalidResult : InvalidResult
    {
        public override List<ValidationError> Reasons { get; }

        public override string Message { get; }
        public override bool Valid { get; }

        public BodyInvalidResult(string content, List<ValidationError> reasons, bool valid)
        {
            Message = Messages.BodyInvalid(content);
            Reasons = reasons;
            Valid = valid;
        }
    }
}
