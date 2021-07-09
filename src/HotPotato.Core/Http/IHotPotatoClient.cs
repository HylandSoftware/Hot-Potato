using System.Threading.Tasks;

namespace HotPotato.Core.Http
{
    public interface IHotPotatoClient
    {
        Task<IHotPotatoResponse> SendAsync(IHotPotatoRequest request);
    }
}
