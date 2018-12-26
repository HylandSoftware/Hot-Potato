using HotPotato.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Middleware
{
    public class ProxyMiddleware
    {
        private const string RemoteEndpointKey = "RemoteEndpoint";

        private readonly IProxy proxy;
        private readonly ILogger log;
        private readonly string remoteEndpoint;
        public ProxyMiddleware(RequestDelegate next, IProxy proxy, IConfiguration config, ILogger<ProxyMiddleware> log)
        {
            _ = proxy ?? throw new ArgumentNullException(nameof(proxy));
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _ = log ?? throw new ArgumentNullException(nameof(log));

            this.proxy = proxy;
            this.log = log;
            this.remoteEndpoint = config[RemoteEndpointKey];
            log.LogInformation($"Forwarding to {remoteEndpoint}");
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.proxy.ProcessAsync(this.remoteEndpoint, context.Request, context.Response);
                this.log.LogDebug("--------------- Request End ---------------");
            }
            catch (Exception e)
            {
                this.log.LogError(e, "Failed to forward request");
                throw;
            }
        }
    }
}
