
using HotPotato.OpenApi.Matchers;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal class PathValidator
    {
        public string Path { get; }
        public SwaggerPathItem Result { get; private set; }

        public PathValidator(string path)
        {
            Path = path ?? "";
        }

        public bool Validate(SwaggerDocument swagDoc)
        {
            if (Path == "")
            {
                return false;
            }
            string match = PathMatcher.Match(Path, swagDoc.Paths.Keys);
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
