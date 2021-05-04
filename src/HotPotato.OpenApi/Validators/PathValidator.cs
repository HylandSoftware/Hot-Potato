using HotPotato.OpenApi.Matchers;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal class PathValidator
    {
        public string Path { get; }
        public OpenApiPathItem Result { get; private set; }

        public PathValidator(string path)
        {
            Path = path ?? string.Empty;
        }

        public bool Validate(OpenApiDocument swagDoc)
        {
            if (string.IsNullOrEmpty(Path))
            {
                return false;
            }
            string match = PathMatcher.Match(Path, swagDoc.Paths.Keys);
            if (match != null)
            {
                if (swagDoc.Paths.ContainsKey(match))
                {
                    Result = swagDoc.Paths[match];
                    return true;
                }
                else
				{
                    string matchWithSlash = match + "/";
                    if (swagDoc.Paths.ContainsKey(matchWithSlash))
                    {
                        Result = swagDoc.Paths[matchWithSlash];
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
