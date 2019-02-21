using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
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
