using HotPotato.Core.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
using static HotPotato.OpenApi.Enums.ReasonEnum;

namespace HotPotato.OpenApi.Results
{
    public interface IResultCollector
    {
        void Pass(HttpPair pair);
        void Fail(HttpPair pair, Reason reason, List<ValidationError> errorReason);
    }
}
