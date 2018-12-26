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
        private const string DELETE = "DELETE";
        private const string GET = "GET";
        private const string OPTIONS = "OPTIONS";
        private const string POST = "POST";
        private const string PUT = "PUT";
        private const string PATCH = "PATCH";
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
                case DELETE:
                    response = await this.Client.Delete(request);
                    break;
                case GET:
                    response = await this.Client.Get(request);
                    break;
                case OPTIONS:
                    response = await this.Client.Options(request);
                    break;
                case PATCH:
                    response = await this.Client.Patch(request);
                    break;
                case POST:
                    response = await this.Client.Post(request);
                    break;
                case PUT:
                    response = await this.Client.Put(request);
                    break;
                default:
                    throw new InvalidHttpVerbException();
            }
            responseOut = response.BuildResponse(responseOut);
        }
    }
}
