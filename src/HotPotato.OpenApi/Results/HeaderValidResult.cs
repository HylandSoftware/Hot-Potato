using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public class HeaderValidResult : Result
    {
        public override string Message { get; }
        public override bool Valid { get; }
        public HeaderValidResult(string key, string value, bool valid)
        {
            Message = Messages.HeaderValueValid(key, value);
            Valid = valid;
        }
    }
}
