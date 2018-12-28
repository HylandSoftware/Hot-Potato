using System;
using System.Collections.Generic;
using System.Text;

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
