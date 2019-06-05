
using HotPotato.OpenApi.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Models
{
    public class FailResult : Result
    {
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<Reason> Reasons { get; }
        public List<ValidationError> ValidationErrors { get; }

        public FailResult(string path, string method, int statusCode, List<Reason> reasons, List<ValidationError> validationErrors)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            State = State.Fail;
            Reasons = reasons;
            ValidationErrors = validationErrors;
        }
    }
}
