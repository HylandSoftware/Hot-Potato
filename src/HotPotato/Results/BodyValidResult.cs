using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class BodyValidResult : Result
    {
        public override string Message { get; }
        public override bool Valid { get; }
        public BodyValidResult(string content, bool valid)
        {
            Message = Messages.BodyValid(content);
            Valid = valid;
        }
    }
}
