using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class HeaderNotFoundResult : Result
    {
        public override string Message { get; }
        public override bool Valid { get; }

        public HeaderNotFoundResult(string key, bool valid)
        {
            Message = Messages.HeaderNotFound(key);
            Valid = valid;
        }
    }
}
