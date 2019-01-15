using System.Threading.Tasks;

namespace HotPotato.Http
{
    public interface IHttpClient
    {
        Task<IHttpResponse> SendAsync(IHttpRequest request);
    }
}
