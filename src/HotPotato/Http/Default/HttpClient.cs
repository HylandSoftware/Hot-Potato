using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.Http.Default
{
    internal class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient client;
        public HttpClient(System.Net.Http.HttpClient client)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));
            this.client = client;
        }

        public async Task<IHttpResponse> SendAsync(IHttpRequest request)
        {
            using (HttpResponseMessage response = await client.SendAsync(request.BuildRequest()))
            {
                return await response.ConvertResponse();
            }
        }
    }
}
