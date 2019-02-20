using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class HeaderNotFoundResult : Result
    {
        public override string Message { get; }
        public override List<ValidationError> Reasons { get; }

        public HeaderNotFoundResult(string key)
        {
            Message = Messages.HeaderNotFound(key);
        }
    }
}
