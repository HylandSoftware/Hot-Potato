using System.Net;
using System.Net.Http.Headers;

namespace HotPotato.Http
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }
        HttpHeaders Headers { get; }
        byte[] Content { get; }
        MediaTypeHeaderValue ContentType { get; }
    }
}
