using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Http
{
    public interface IHttpClient
    {
        Task<IHttpResponse> Get(IHttpRequest request);
        Task<IHttpResponse> Post(IHttpRequest request);
        Task<IHttpResponse> Put(IHttpRequest request);
        Task<IHttpResponse> Delete(IHttpRequest request);
        Task<IHttpResponse> Patch(IHttpRequest request);
        Task<IHttpResponse> Options(IHttpRequest request);
    }
}
