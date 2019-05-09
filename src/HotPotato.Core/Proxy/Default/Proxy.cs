﻿using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotPotato.Core.Proxy.Default
{
    public class Proxy : IProxy
    {

        private IHttpClient Client { get; }
        private ILogger Logger { get; }
        private IProcessor Processor { get; }

        public Proxy(IHttpClient client, ILogger<Proxy> logger, IProcessor processor)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));
            _ = logger ?? throw new ArgumentNullException(nameof(logger));
            _ = processor ?? throw new ArgumentNullException(nameof(processor));

            this.Client = client;
            this.Logger = logger;
            this.Processor = processor;
        }

        public async Task ProcessAsync(string remoteEndpoint, HttpRequest requestIn, HttpResponse responseOut)
        {
            using (IHttpRequest request = requestIn.ToProxyRequest(remoteEndpoint))
            {
                IHttpResponse response = await this.Client.SendAsync(request);
                await response.ToProxyResponseAsync(responseOut);
                using (HttpPair pair = new HttpPair(request, response))
                {
                    this.Processor.Process(pair);
                }
            }
        }
    }
}
