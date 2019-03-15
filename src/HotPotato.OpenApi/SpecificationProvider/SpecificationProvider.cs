using Microsoft.Extensions.Configuration;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotPotato.OpenApi.SpecificationProvider
{
    internal class SpecificationProvider : ISpecificationProvider
    {
        private readonly string specLoc;
        public SpecificationProvider(IConfiguration config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            this.specLoc = config["SpecLocation"].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
        public SwaggerDocument GetSpecDocument()
        {
            Task<SwaggerDocument> swagTask = null;
            if (Path.IsPathFullyQualified(specLoc))
            {
                swagTask = FromFileAsync(specLoc);
            }
            else if (Uri.IsWellFormedUriString(specLoc, UriKind.Absolute))
            {
                swagTask = FromUrlAsync(specLoc);
                var test = swagTask.Result;
            }
            else
            {
                throw new InvalidOperationException("AppSettings does not contain a valid Spec Location");
            }
            return swagTask.Result;
        }
    }
}
