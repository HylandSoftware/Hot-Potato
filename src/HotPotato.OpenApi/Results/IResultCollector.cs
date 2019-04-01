using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public interface IResultCollector
    {
        State OverallResult { get; }
        List<Models.Result> Results { get; }
        void Pass(HttpPair pair);
        void Fail(HttpPair pair, Reason reason, params ValidationError[] validationErrors);
    }
}
