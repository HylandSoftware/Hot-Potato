
namespace HotPotato.Core.Http
{
    public class HttpContentType
    {
        private const string DefaultCharSet = "utf-8";

        public string Type { get; }
        public string CharSet { get; }

        public HttpContentType(string type, string charSet = DefaultCharSet)
        {
            if (type.Contains(";"))
            {
                //Sanitize content-types for uniform matching later on
                Type = type.Split(";")[0];
            }
            else
            {
                Type = type;
            }

            if (string.IsNullOrWhiteSpace(charSet))
            {
                CharSet = DefaultCharSet;
            }
            else
            {
                CharSet = charSet;
            }
        }
    }
}
