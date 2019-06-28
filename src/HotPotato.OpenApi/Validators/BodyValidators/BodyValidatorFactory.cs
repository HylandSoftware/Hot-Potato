using HotPotato.Core.Http;

namespace HotPotato.OpenApi.Validators
{
    internal static class BodyValidatorFactory
    {
        public static BodyValidator Create(string bodyString, HttpContentType contentType)
        {
            if (contentType.Type.ToLower().Contains(BodyValidatorContentTypes.json))
            {
                return new JsonBodyValidator(bodyString);
            }
            else if (contentType.Type.ToLower().Contains(BodyValidatorContentTypes.xml))
            {
                return new XmlBodyValidator(bodyString);
            }
            else
            {
                return new TextBodyValidator(bodyString);
            }
        }
    }
}
