using System.Net;
using System.Net.Http.Headers;

namespace HotPotato.Core.Http
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }
        HttpHeaders Headers { get; }
        byte[] Content { get; }
        HttpContentType ContentType { get; }
    }
}
