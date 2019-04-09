
using HotPotato.OpenApi.Matchers;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal class PathValidator
    {
        public string path { get; }
        public SwaggerPathItem Result { get; set; }

        public PathValidator(string Path)
        {
            path = Path ?? "";
        }

        public bool Validate(SwaggerDocument swagDoc)
        {
            if (path == "")
            {
                return false;
            }
            string match = PathMatcher.Match(path, swagDoc.Paths.Keys);
            if (swagDoc.Paths.ContainsKey(match))
            {
                Result = swagDoc.Paths[match];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
