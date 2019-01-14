using HotPotato.Http;
using System;

namespace HotPotato.Models
{
    public class HttpPair
    {
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        public HttpPair(IHttpRequest request, IHttpResponse response)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _ = response ?? throw new ArgumentNullException(nameof(response));

            this.Request = request;
            this.Response = response;
        }
    }
}
