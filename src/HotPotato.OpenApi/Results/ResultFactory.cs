using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public static class ResultFactory
    {
        //Setting HeaderNotFoundResult's Valid param to true since it doesn't implement the InvalidResult class or the Results collection
        public static HeaderNotFoundResult HeaderNotFoundResult(string key) => new HeaderNotFoundResult(key, true);
        public static HeaderInvalidResult HeaderInvalidResult(string key, string value, List<Validators.ValidationError> errors) => new HeaderInvalidResult(key, value, errors, false);
        public static HeaderValidResult HeaderValidResult(string key, string value) => new HeaderValidResult(key, value, true);
        public static BodyValidResult BodyValidResult(string content) => new BodyValidResult(content, true);
        public static BodyInvalidResult BodyInvalidResult(string content, List<Validators.ValidationError> errors) => new BodyInvalidResult(content, errors, false);

    }
}
