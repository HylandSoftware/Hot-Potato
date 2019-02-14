using HotPotato.Validators;
using NJsonSchema.Validation;
using System.Collections.Generic;
using System.Linq;

namespace HotPotato.Results
{
    public static class ResultFactory
    {
        public static HeaderNotFoundResult HeaderNotFoundResult(string key) => new HeaderNotFoundResult(key);
        public static HeaderInvalidResult HeaderInvalidResult(string key, string value, List<HotPotatoValidationError> errors) => new HeaderInvalidResult(key, value, errors);
        public static HeaderValidResult HeaderValidResult(string key, string value) => new HeaderValidResult(key, value);
        public static BodyValidResult BodyValidResult(string content) => new BodyValidResult(content);
        public static BodyInvalidResult BodyInvalidResult(string content, List<HotPotatoValidationError> errors) => new BodyInvalidResult(content, errors);

    }
}
