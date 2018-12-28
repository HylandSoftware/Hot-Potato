using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Results
{
    public class HeaderNotFoundResult : Result
    {
        public override string Message { get; }

        public HeaderNotFoundResult(string key)
        {
            Message = Messages.HeaderNotFound(key);
        }
    }
}
