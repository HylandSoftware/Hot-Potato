using System.Collections.Generic;

namespace HotPotato.Results
{
    public abstract class InvalidResult : Result
    {
        public abstract IEnumerable<string> Reasons { get; }
    }
}
