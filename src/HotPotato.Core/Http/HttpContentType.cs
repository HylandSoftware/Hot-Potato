using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Core.Http
{
    public class HttpContentType
    {
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
                CharSet = "utf-8";
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
