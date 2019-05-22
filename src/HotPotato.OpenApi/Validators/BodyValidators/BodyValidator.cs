using HotPotato.Core.Http;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal abstract class BodyValidator
    {
        public string BodyString { get; protected set; }
        public HttpContentType ContentType { get; protected set; }

        public abstract IValidationResult Validate(SwaggerResponse swagResp);

        protected virtual JsonSchema4 GetSchema(SwaggerResponse swagResp)
        {
            JsonSchema4 specBody = swagResp.ActualResponse.Schema;

            //Conditional for matching schemas with multiple content-type returns
            if (swagResp.Content != null && swagResp.Content.Count > 0)
            {
                Dictionary<string, OpenApiMediaType> contentSchemas = swagResp.Content.SanitizeContentTypes();
                if (contentSchemas.ContainsKey(ContentType.Type))
                {
                    specBody = contentSchemas[ContentType.Type].Schema;
                }
                return specBody;
            }
            else
            {
                return specBody;
            }
        }
    }
}
