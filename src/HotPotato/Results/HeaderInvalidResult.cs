using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Results
{
    public class HeaderInvalidResult : InvalidResult
    {
        public override string Message { get; }
        public override IEnumerable<string> Reasons { get; }

        public HeaderInvalidResult(string key, string value, IEnumerable<string> reasons)
        {
            Message = Messages.HeaderValueInvalid(key, value);
            Reasons = reasons;
        }
    }
}
