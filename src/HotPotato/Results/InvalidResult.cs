using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Results
{
    public abstract class InvalidResult : Result
    {
        public abstract IEnumerable<string> Reasons { get; }
    }
}
