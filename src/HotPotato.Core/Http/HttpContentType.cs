
namespace HotPotato.Core.Http
{
    public class HttpContentType
    {
        private const string DefaultCharSet = "utf-8";

        public string Type { get; }
        public string CharSet { get; }

        public HttpContentType(string type, string charSet)
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

        public HttpContentType(string type)
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
            CharSet = "utf-8";
        }
    }
}
