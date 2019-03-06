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
        public List<ValidationError> ErrorReason { get; }
        
    }
}
