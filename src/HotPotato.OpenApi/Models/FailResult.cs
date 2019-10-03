
using HotPotato.OpenApi.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Models
{
    public class FailResult : Result
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public override State State => State.Fail;
        public override string Path { get; protected set; }
        public override string Method { get; protected set; }
        public override int StatusCode { get; protected set; }
        /// <summary>
        /// Properties above this overriden to ensure the two error properties are last in serialization
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<Reason> Reasons { get; }
        public List<ValidationError> ValidationErrors { get;  }

        public FailResult(string path, string method, int statusCode, List<Reason> reasons, List<ValidationError> validationErrors)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            Reasons = reasons;
            ValidationErrors = validationErrors;
        }
    }
}
