using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public class FailResult : Result
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Reason Reason { get; }
        public List<ValidationError> ValidationErrors { get; }

        public FailResult(string path, string method, int statusCode, Reason reason, List<ValidationError> validationErrors)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            State = State.Fail;
            Reason = reason;
            ValidationErrors = validationErrors;
        }
    }
}
