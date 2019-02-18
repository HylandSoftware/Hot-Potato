using System.Net;

namespace HotPotato.Core.Http.Default
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode, HttpHeaders headers)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
        }
        public HttpResponse(HttpStatusCode statusCode, HttpHeaders headers, byte[] content)
            : this(statusCode, headers)
        {
            this.Content = content;
        }
        public HttpStatusCode StatusCode { get; }

        public HttpHeaders Headers { get; }

        public byte[] Content { get; }
    }
}
