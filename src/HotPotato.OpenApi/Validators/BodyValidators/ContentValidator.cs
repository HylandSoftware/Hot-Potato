using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;

namespace HotPotato.OpenApi.Validators
{
    internal class ContentValidator
    {
        private string BodyString { get;}
        private HttpContentType ContentType { get; }

        public ContentValidator(string bodyString, HttpContentType contentType)
        {
            BodyString = bodyString;
            ContentType = contentType;
        }
        /// <summary>
        /// In the case of an empty schema in the spec, an empty response body should be expected
        /// However, if one is empty and the other is populated, then an error should be thrown
        /// </summary>
        public IValidationResult Validate(JsonSchema4 schema)
        {
            if (schema == null && string.IsNullOrWhiteSpace(BodyString))
            {
                return new ValidResult();
            }
            else if (schema == null)
            {
                return new InvalidResult(Reason.MissingContentType, ContentProvider.GenerateContentError(ContentType.Type));
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
