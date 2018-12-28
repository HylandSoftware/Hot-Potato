using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HotPotato.Http
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }
        HttpHeaders Headers { get; }
        byte[] Content { get; }
    }
}
