using NJsonSchema.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HotPotato.Results
{
    public static class ResultFactory
    {
        public static HeaderNotFoundResult HeaderNotFoundResult(string key) => new HeaderNotFoundResult(key);
        public static HeaderInvalidResult HeaderInvalidResult(string key, string value, ICollection<ValidationError> errors) => new HeaderInvalidResult(key, value, errors.Select((r) => r.ToString()));
        public static HeaderValidResult HeaderValidResult(string key, string value) => new HeaderValidResult(key, value);
        public static BodyValidResult BodyValidResult(string content) => new BodyValidResult(content);
        public static BodyInvalidResult BodyInvalidResult(string content, ICollection<ValidationError> errors) => new BodyInvalidResult(content, errors.Select((r) => r.ToString()));

    }
}
