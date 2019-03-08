using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using NSwag;

namespace HotPotato.OpenApi.Validators.NSwag
{
    internal class PathValidator
    {
        public void Validate(SwaggerPathItem pathItem, HttpPair pair, IResultCollector collector)
        {
            if (pathItem == null)
            {
                collector.Fail(pair, Reason.MissingPath);
            }
            else
            {
                collector.Pass(pair);
            }
        }
    }
}
