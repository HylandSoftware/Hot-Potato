using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.Core.Http.Default
{
    public class HotPotatoClient : IHotPotatoClient
    {
        private readonly System.Net.Http.HttpClient client;

        public HotPotatoClient(System.Net.Http.HttpClient client)
        {
            _ = client ?? throw Exceptions.ArgumentNull(nameof(client));
            this.client = client;
        }

        public async Task<IHotPotatoResponse> SendAsync(IHotPotatoRequest request)
        {
            using (HttpResponseMessage response = await client.SendAsync(request.ToClientRequestMessage()))
            {
                return await response.ToClientResponseAsync();
            }
        }
    }
}
