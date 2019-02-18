using System.Threading.Tasks;

namespace HotPotato.Core.Http
{
    public interface IHttpClient
    {
        Task<IHttpResponse> SendAsync(IHttpRequest request);
    }
}
