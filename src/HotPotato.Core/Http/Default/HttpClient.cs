using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.Core.Http.Default
{
    public class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient client;

        public HttpClient(System.Net.Http.HttpClient client)
        {
            _ = client ?? throw Exceptions.ArgumentNull(nameof(client));
            this.client = client;
        }

        public async Task<IHttpResponse> SendAsync(IHttpRequest request)
        {
            using (HttpResponseMessage response = await client.SendAsync(request.ToClientRequestMessage()))
            {
                return await response.ToClientResponseAsync();
            }
        }
    }
}
