
using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

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
            JsonSchema4 specBody = swagResp.ActualResponse.Schema;

            //Conditional for matching schemas with multiple content-type returns
            if (swagResp.Content != null && swagResp.Content.Count > 0)
            {
                Dictionary<string, OpenApiMediaType> contentSchemas = swagResp.Content.SanitizeContentTypes();
                if (contentSchemas.ContainsKey(ContentType.Type))
                {
                    specBody = contentSchemas[ContentType.Type].Schema;
                }
            }

            if (specBody == null)
            {
                return new InvalidResult(Reason.MissingSpecBody);
            }
            else if (string.IsNullOrWhiteSpace(BodyString))
            {
                return new InvalidResult(Reason.MissingBody);
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
