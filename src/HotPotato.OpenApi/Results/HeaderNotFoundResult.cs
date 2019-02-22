using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
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
