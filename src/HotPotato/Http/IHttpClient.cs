using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Http
{
    public interface IHttpClient
    {
        Task<IHttpResponse> SendAsync(IHttpRequest request);
    }
}
