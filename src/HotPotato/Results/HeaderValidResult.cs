using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class HeaderValidResult : Result
    {
        public override string Message { get; }
        public override List<HotPotatoValidationError> Reasons { get; }

        public HeaderValidResult(string key, string value)
        {
            Message = Messages.HeaderValueValid(key, value);
        }
    }
}
