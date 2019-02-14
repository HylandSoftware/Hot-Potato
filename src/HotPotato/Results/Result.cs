using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public abstract class Result
    {
        public abstract string Message { get; }
        public abstract List<HotPotatoValidationError> Reasons { get; }
        public override string ToString() => Message;
    }
}
