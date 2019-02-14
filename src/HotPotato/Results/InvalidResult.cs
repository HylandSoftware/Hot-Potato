using HotPotato.Validators;
using System.Collections.Generic;

namespace HotPotato.Results
{
    public abstract class InvalidResult : Result
    {
        public override List<HotPotatoValidationError> Reasons { get; }
    }
}
