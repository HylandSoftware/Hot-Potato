using System.Net;
using System.Net.Http.Headers;

namespace HotPotato.Http.Default
{
    internal class HttpResponse : IHttpResponse
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
        public MediaTypeHeaderValue ContentType { get; }
    }
}
