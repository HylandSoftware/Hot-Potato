
using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    internal class XmlBodyValidator : BodyValidator
    {
        public XmlBodyValidator(string bodyString)
        {
            BodyString = bodyString;
        }

        public override IValidationResult Validate(JsonSchema4 schema)
        {
            var xmlErrList = schema.ValidateXml(BodyString);
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
