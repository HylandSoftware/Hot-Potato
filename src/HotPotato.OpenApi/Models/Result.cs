using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Models
{
    public class Result
    {
        public string Path { get; }
        public string Method { get; }
        public int StatusCode { get; }
        public State State { get; }
        public Reason Reason { get; }
        public List<ValidationError> ValidationErrors { get; }

        public Result(string path, string method, int statusCode, State state)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            State = state;
        }

        public Result(string path, string method, int statusCode, State state, Reason reason, List<ValidationError> validationErrors)
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
