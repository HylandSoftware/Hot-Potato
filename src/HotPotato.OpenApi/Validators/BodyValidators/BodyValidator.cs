using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal abstract class BodyValidator
    {
        public string BodyString { get; protected set; }
        public HttpContentType ContentType { get; protected set; }

        public abstract IValidationResult Validate(SwaggerResponse swagResp);

        /// <summary>
        /// If both spec body and response body are empty, then an empty body should be expected
        /// However, if one is empty and the other is populated, then an error should be thrown
        /// </summary>
        protected IValidationResult ValidateMissingContent(JsonSchema4 specBody)
        {
            if (specBody == null && string.IsNullOrWhiteSpace(BodyString))
            {
                return new ValidResult();
            }
            else if (specBody == null)
            {
                return new InvalidResult(Reason.MissingContent, ContentProvider.GenerateContentError(ContentType.Type));
            }
            else if (string.IsNullOrWhiteSpace(BodyString))
            {
                return new InvalidResult(Reason.MissingBody);
            }
            else
            {
                return null;
            }
        }
    }
}
