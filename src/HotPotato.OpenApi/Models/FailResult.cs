using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Models
{
    public class FailResult : Result
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Reason Reason { get; }
        public List<ValidationError> ValidationErrors { get; }

        public FailResult(string path, string method, int statusCode, State state, Reason reason, List<ValidationError> validationErrors)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            State = state;
            Reason = reason;
            ValidationErrors = validationErrors;
        }
    }
}
