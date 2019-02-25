using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public abstract class InvalidResult : Result
    {
        public override bool Valid { get; }
        public abstract List<ValidationError> Reasons { get; }
    }
}
