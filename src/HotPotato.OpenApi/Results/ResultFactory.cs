using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
using System.Linq;

namespace HotPotato.OpenApi.Results
{
    public static class ResultFactory
    {
        //Setting HeaderNotFoundResult's Valid param to true since it doesn't implement the InvalidResult class or the Results collection
        public static HeaderNotFoundResult HeaderNotFoundResult(string key) => new HeaderNotFoundResult(key, true);
        public static HeaderInvalidResult HeaderInvalidResult(string key, string value, List<ValidationError> errors) => new HeaderInvalidResult(key, value, errors, false);
        public static HeaderValidResult HeaderValidResult(string key, string value) => new HeaderValidResult(key, value, true);
        public static BodyValidResult BodyValidResult(string content) => new BodyValidResult(content, true);
        public static BodyInvalidResult BodyInvalidResult(string content, List<ValidationError> errors) => new BodyInvalidResult(content, errors, false);
        public static Models.Result PassResult(string path, string method, int statusCode, State state) =>
            new Models.Result(path, method, statusCode, state);
        public static Models.Result FailResult(string path, string method, int statuCode, State state, Reason reason, params ValidationError[] validationErrors) =>
            new Models.Result(path, method, statuCode, state, reason, validationErrors.ToList());
    }
}
