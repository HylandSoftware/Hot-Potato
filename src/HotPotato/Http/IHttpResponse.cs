using System.Net;

namespace HotPotato.Http
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }
        HttpHeaders Headers { get; }
        byte[] Content { get; }
    }
}
