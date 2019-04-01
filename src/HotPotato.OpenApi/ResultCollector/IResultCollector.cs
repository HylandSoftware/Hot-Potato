using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public interface IResultCollector
    {
        List<Models.Result> Results { get; }
        void Pass(string path, string method, int statusCode);
        void Fail(string path, string method, int StatusCode, Reason reason, ValidationError[] validationErrors);
    }
}
