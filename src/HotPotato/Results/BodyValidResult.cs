using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public class BodyValidResult : Result
    {
        public override string Message { get; }
        public override List<HotPotatoValidationError> Reasons { get; }
        public BodyValidResult(string content)
        {
            Message = Messages.BodyValid(content);
        }
    }
}
