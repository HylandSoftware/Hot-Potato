using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HotPotato.Core.Http
{
    public interface IHttpRequest : IDisposable
    {
        HttpMethod Method { get; }
        Uri Uri { get; }
        HttpHeaders HttpHeaders { get; }
        HttpHeaders CustomHeaders { get; }
        MediaTypeHeaderValue ContentType { get; }
        HttpContent Content { get; }
        IHttpRequest SetContent(string content);
        IHttpRequest SetContent(string content, Encoding encoding);
        IHttpRequest SetContent(string content, Encoding encoding, string mediaType);
        IHttpRequest SetContent(string content, Encoding encoding, MediaTypeHeaderValue mediaType);
    }
}
