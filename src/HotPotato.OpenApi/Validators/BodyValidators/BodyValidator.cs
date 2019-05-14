using HotPotato.Core.Http;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal abstract class BodyValidator
    {
        public string BodyString { get; protected set; }
        public HttpContentType ContentType { get; protected set; }

        public abstract IValidationResult Validate(SwaggerResponse swagResp);

        /// <summary>
        /// Remove all trailing encodings from content-types for uniform matching
        /// </summary>
        /// <param name="Content">A SwaggerResponse.Content dict</param>
        /// <returns>Content dict with sanitized content-types</returns>
        protected Dictionary<string, OpenApiMediaType> SanitizeContentTypes(IDictionary<string, OpenApiMediaType> Content)
        {
            Dictionary<string, OpenApiMediaType> returnDict = new Dictionary<string, OpenApiMediaType>();
            foreach (KeyValuePair<string, OpenApiMediaType> kvp in Content)
            {
                if (kvp.Key.Contains(";"))
                {
                    returnDict.Add(kvp.Key.Split(";")[0], kvp.Value);
                }
                else
                {
                    returnDict.Add(kvp.Key, kvp.Value);
                }
            }
            return returnDict;
        }
    }
}
