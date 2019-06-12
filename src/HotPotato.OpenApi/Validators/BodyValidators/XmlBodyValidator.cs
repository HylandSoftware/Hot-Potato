
using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal class XmlBodyValidator : BodyValidator
    {
        public XmlBodyValidator(string bodyString, HttpContentType contentType)
        {
            BodyString = bodyString;
            ContentType = contentType;
        }

        public override IValidationResult Validate(SwaggerResponse swagResp)
        {
            JsonSchema4 specBody = ContentProvider.GetSchema(swagResp, ContentType.Type);

            IValidationResult missingContentResult = ValidateMissingContent(specBody);
            if (missingContentResult != null)
            {
                return missingContentResult;
            }

            var xmlErrList = specBody.ValidateXml(BodyString);
            if (xmlErrList.Count == 0)
            {
                return new ValidResult();
            }
            else
            {
                ValidationError[] xmlErrorArr = xmlErrList.ToArray();
                return new InvalidResult(Reason.InvalidBody, xmlErrorArr);
            }
        }
    }
}
