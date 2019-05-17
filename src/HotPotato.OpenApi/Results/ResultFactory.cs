using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Linq;

namespace HotPotato.OpenApi.Results
{
    public static class ResultFactory
    {
        public static Result PassResult(string path, string method, int statusCode, State state) =>
            new PassResult(path, method, statusCode, state);
        public static Result FailResult(string path, string method, int statusCode, State state, Reason reason, params ValidationError[] validationErrors) =>
            new FailResult(path, method, statusCode, state, reason, validationErrors?.ToList());
    }
}
