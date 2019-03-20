using HotPotato.Core.Models;
using HotPotato.OpenApi.Matchers;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using NSwag;
using System;

namespace HotPotato.OpenApi.Validators
{
    internal class PathValidator : IValidator
    {
        private readonly IValidationProvider valPro;
        private readonly IResultCollector collector;
        public PathValidator(IValidationProvider valPro, IResultCollector collector)
        {
            this.valPro = valPro;
            this.collector = collector;
        }
        public void Validate(HttpPair pair)
        {
            _ = pair ?? throw new ArgumentNullException(nameof(pair));

            SwaggerDocument swagDoc = valPro.specDoc;
            string match = PathMatcher.Match(pair.Request.Uri.AbsolutePath, swagDoc.Paths.Keys);
            if (swagDoc.Paths.ContainsKey(match))
            {
                valPro.specPath = swagDoc.Paths[match];
            }
            else
            {
                collector.Fail(pair, Reason.MissingPath);
            }
        }
    }
}
