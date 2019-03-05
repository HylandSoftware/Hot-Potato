using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
using static HotPotato.OpenApi.Enums.ReasonEnum;
using static HotPotato.OpenApi.Enums.StateEnum;

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
