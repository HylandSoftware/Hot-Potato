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
        private readonly string SpecLocation;
        public SpecificationProvider(IConfiguration config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            this.SpecLocation = config["SpecLocation"];
        }
        public SwaggerDocument GetSpecDocument()
        {
            Task<SwaggerDocument> swagTask = null;
            if (Path.IsPathFullyQualified(SpecLocation))
            {
                swagTask = FromFileAsync(SpecLocation);
            }
            else if (Uri.IsWellFormedUriString(SpecLocation, UriKind.Absolute))
            {
                swagTask = FromUrlAsync(SpecLocation);
            }
            else
            {
                throw new InvalidOperationException("AppSettings does not contain a valid Spec Location");
            }
            return swagTask.Result;
        }
    }
}
