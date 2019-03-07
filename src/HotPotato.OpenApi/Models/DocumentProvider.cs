using Microsoft.Extensions.Configuration;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.Threading.Tasks;

namespace HotPotato.OpenApi.Models
{
    public class DocumentProvider : IDocumentProvider
    {
        private readonly string specLoc;
        public DocumentProvider(IConfiguration config)
        {
            _ = config ?? throw new ArgumentNullException(nameof(config));
            this.specLoc = config["SpecLocation"];
        }
        public SwaggerDocument GetSpecDocument()
        {
            Task<SwaggerDocument> swagTask = FromFileAsync(specLoc);
            return swagTask.Result;
        }
    }
}
