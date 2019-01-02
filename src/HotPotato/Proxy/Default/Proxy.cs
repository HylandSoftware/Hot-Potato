using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotPotato.Exceptions;
using HotPotato.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
            string method = requestIn.Method.ToUpperInvariant();
            IHttpRequest request = requestIn.ToRequest(remoteEndpoint);
            IHttpResponse response;
            switch (method)
            {
                case HttpVerbs.DELETE:
                    response = await this.Client.Delete(request);
                    break;
                case HttpVerbs.GET:
                    response = await this.Client.Get(request);
                    break;
                case HttpVerbs.OPTIONS:
                    response = await this.Client.Options(request);
                    break;
                case HttpVerbs.PATCH:
                    response = await this.Client.Patch(request);
                    break;
                case HttpVerbs.POST:
                    response = await this.Client.Post(request);
                    break;
                case HttpVerbs.PUT:
                    response = await this.Client.Put(request);
                    break;
                default:
                    throw new InvalidHttpVerbException();
            }
            await response.BuildResponse(responseOut);
        }
    }
}
