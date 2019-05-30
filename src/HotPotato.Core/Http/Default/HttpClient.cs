using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.Core.Http.Default
{
    public class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient client;
        //public HttpClient(System.Net.Http.HttpClient client)
        //{
        //    _ = client ?? throw Exceptions.ArgumentNull(nameof(client));
        //    this.client = client;

        //}
        public HttpClient(System.Net.Http.HttpClient client, IWebProxy proxy = null, ForwardProxy.HttpForwardProxyConfig proxyConfig = null)
        {
            _ = client ?? throw Exceptions.ArgumentNull(nameof(client));
            if (proxyConfig != null && proxyConfig.Enabled)
            { 
                System.Net.Http.HttpClientHandler handler = new System.Net.Http.HttpClientHandler
                {
                    Proxy = proxy
                };
                this.client = new System.Net.Http.HttpClient(handler);
            }
            else
            {
                this.client = client;
            }
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
