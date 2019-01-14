using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HotPotato.Proxy
{
    public interface IProxy
    {
        Task ProcessAsync(string remoteEndpoint, HttpRequest requestIn, HttpResponse responseOut);
    }
}
