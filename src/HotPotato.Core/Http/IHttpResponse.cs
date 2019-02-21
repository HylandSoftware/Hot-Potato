using System.Net;

namespace HotPotato.Core.Http
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }
        HttpHeaders Headers { get; }
        byte[] Content { get; }
    }
}
