using HotPotato.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotPotato.Proxy.Default
{
    public class Proxy : IProxy
    {

        private IHttpClient Client { get; }
        private ILogger Logger { get; }

        public Proxy(IHttpClient client, ILogger<Proxy> logger)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));
            _ = logger ?? throw new ArgumentNullException(nameof(logger));

            this.Client = client;
            this.Logger = logger;
        }

        public async Task ProcessAsync(string remoteEndpoint, HttpRequest requestIn, HttpResponse responseOut)
        {
            IHttpRequest request = requestIn.ToRequest(remoteEndpoint);
            IHttpResponse response = await this.Client.SendAsync(request);
            await response.BuildResponse(responseOut);
        }
    }
}
