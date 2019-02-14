using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class HeaderInvalidResult : InvalidResult
    {
        public override string Message { get; }
        public override List<HotPotatoValidationError> Reasons { get; }

        public HeaderInvalidResult(string key, string value, List<HotPotatoValidationError> reasons)
        {
            Message = Messages.HeaderValueInvalid(key, value);
            Reasons = reasons;
        }
    }
}
