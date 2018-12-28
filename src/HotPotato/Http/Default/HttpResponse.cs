using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HotPotato.Http.Default
{
    internal class HttpResponse : IHttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode, HttpHeaders headers, byte[] content)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
            this.Content = content;
        }
        public HttpStatusCode StatusCode { get; }

        public HttpHeaders Headers { get; }

        public byte[] Content { get; }
    }
}
