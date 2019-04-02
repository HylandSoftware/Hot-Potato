using Microsoft.Extensions.Configuration;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class SpecificationProvider : ISpecificationProvider
    {
        private readonly string specLoc;
        public SpecificationProvider(IConfiguration config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            this.specLoc = config["SpecLocation"];
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
            }
            else
            {
                throw new InvalidOperationException("AppSettings does not contain a valid Spec Location");
            }
            return swagTask.Result;
        }
    }
}
