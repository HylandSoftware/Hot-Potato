using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HotPotato.Core.Proxy
{
    public interface IProxy
    {
        Task ProcessAsync(string remoteEndpoint, Microsoft.AspNetCore.Http.HttpRequest requestIn, Microsoft.AspNetCore.Http.HttpResponse responseOut);
    }
}
