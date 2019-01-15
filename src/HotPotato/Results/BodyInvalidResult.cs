using System.Collections.Generic;

namespace HotPotato.Results
{
    public class BodyInvalidResult : InvalidResult
    {
        public override IEnumerable<string> Reasons { get; }

        public override string Message { get; }

        public BodyInvalidResult(string content, IEnumerable<string> reasons)
        {
            Message = Messages.BodyInvalid(content);
            Reasons = reasons;
        }
    }
}
