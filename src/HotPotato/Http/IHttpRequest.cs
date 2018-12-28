using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HotPotato.Http
{
    public interface IHttpRequest
    {
        string Method { get; }
        Uri Uri { get; set; }
        HttpHeaders HttpHeaders { get; set; }
        MediaTypeHeaderValue ContentType { get; set; }
        HttpContent Content { get; }
        IHttpRequest SetContent(string content);
        IHttpRequest SetContent(string content, Encoding encoding);
        IHttpRequest SetContent(string content, Encoding encoding, string mediaType);
        IHttpRequest SetContent(string content, Encoding encoding, MediaTypeHeaderValue mediaType);
    }
}
