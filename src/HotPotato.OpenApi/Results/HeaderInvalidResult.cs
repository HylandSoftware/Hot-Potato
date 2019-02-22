using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public class HeaderInvalidResult : InvalidResult
    {
        public override string Message { get; }
        public override List<ValidationError> Reasons { get; }
        public override bool Valid { get; }

        public HeaderInvalidResult(string key, string value, List<ValidationError> reasons, bool valid)
        {
            Message = Messages.HeaderValueInvalid(key, value);
            Reasons = reasons;
            Valid = valid;
        }
    }
}
