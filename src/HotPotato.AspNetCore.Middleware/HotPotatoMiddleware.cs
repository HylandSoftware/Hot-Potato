using HotPotato.Core.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.AspNetCore.Middleware
{
    public class HotPotatoMiddleware
    {
        private const string RemoteEndpointKey = "RemoteEndpoint";

        private readonly IProxy proxy;
        private readonly ILogger log;
        private readonly string remoteEndpoint;
        private readonly RequestDelegate _next;
        
        public HotPotatoMiddleware(RequestDelegate next, IProxy proxy, IConfiguration configuration, ILogger<HotPotatoMiddleware> log)
        {
            _ = proxy ?? throw new ArgumentNullException(nameof(proxy));
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _ = log ?? throw new ArgumentNullException(nameof(log));

            this.proxy = proxy;
            this.log = log;
            this.remoteEndpoint = configuration[RemoteEndpointKey];
            log.LogInformation($"Forwarding to {remoteEndpoint}");
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path.Value == "/results")
            {
                await _next.Invoke(context);
            }
           else
	       {
                try
                {
                    this.log.LogDebug($"{context.Request.Method} {context.Request.Path}");
                    await this.proxy.ProcessAsync(this.remoteEndpoint, context.Request, context.Response);
                    this.log.LogDebug($"{context.Response.StatusCode} Length: {context.Response.ContentLength}");
                    this.log.LogDebug("--------------- Request End ---------------");
                }
                catch (HttpRequestException httpEx)
                {
                    this.log.LogError(httpEx, "Failed to forward request. Remote endpoint may be down.");
                    context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                }
                catch (Exception e)
                {
                    this.log.LogError(e, "Failed to forward request");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                } 
           }
        }
    }
}
