using HotPotato.Core.Http;

namespace HotPotato.OpenApi.Validators
{
    internal static class BodyValidatorFactory
    {
        public static BodyValidator Create(string bodyString, HttpContentType contentType)
        {
            if (contentType.Type.ToLower().Contains("json"))
            {
                return new JsonBodyValidator(bodyString, contentType);
            }
            else if (contentType.Type.ToLower().Contains("xml"))
            {
                return new XmlBodyValidator(bodyString, contentType);
            }
            else
            {
                return new TextBodyValidator(bodyString, contentType);
            }
        }
    }
}
