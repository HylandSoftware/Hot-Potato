using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Models
{
    public class PassResult : Result
    {
        public PassResult(string path, string method, int statusCode, State state)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            State = state;
        }
    }
}
