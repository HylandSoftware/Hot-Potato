using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Linq;

namespace HotPotato.OpenApi.Results
{
    public static class ResultFactory
    {
        public static Models.Result PassResult(string path, string method, int statusCode, State state) =>
            new Models.Result(path, method, statusCode, state);
        public static Models.Result FailResult(string path, string method, int statusCode, State state, Reason reason, params ValidationError[] validationErrors) =>
            new Models.Result(path, method, statusCode, state, reason, validationErrors?.ToList());
    }
}
