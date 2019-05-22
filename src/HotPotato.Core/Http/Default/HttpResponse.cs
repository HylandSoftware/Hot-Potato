
using System.Net;
using System.Net.Http.Headers;

namespace HotPotato.Core.Http.Default
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode, HttpHeaders headers)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
        }
        public HttpResponse(HttpStatusCode statusCode, HttpHeaders headers, byte[] content, MediaTypeHeaderValue contentType)
            : this(statusCode, headers)
        {
            this.Content = content;
            this.ContentType = new HttpContentType(contentType.MediaType, contentType.CharSet);
        }
        public HttpStatusCode StatusCode { get; }

        public HttpHeaders Headers { get; }

        public byte[] Content { get; }

        public HttpContentType ContentType { get; }

    }
}
