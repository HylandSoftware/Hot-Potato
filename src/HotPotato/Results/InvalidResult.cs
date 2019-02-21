using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public abstract class InvalidResult : Result
    {
        public override bool Valid { get; }
        public abstract List<ValidationError> Reasons { get; }
    }
}
