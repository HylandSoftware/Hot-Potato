using HotPotato.Core;
using HotPotato.Core.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.AspNetCore.Middleware
{
	public class HotPotatoMiddleware
	{
		private const string RemoteEndpointKey = "RemoteEndpoint";
		private const string SpecLocationKey = "SpecLocation";

		private readonly IProxy proxy;
		private readonly ILogger log;
		private readonly string remoteEndpoint;
		private readonly string specLocation;
		private readonly RequestDelegate _next;

		private static readonly HashSet<string> ProxyEndpoints =
			new HashSet<string>
			{
				"/results",
				"/cookies"
			};

		public HotPotatoMiddleware(RequestDelegate next, IProxy proxy, IConfiguration configuration, ILogger<HotPotatoMiddleware> log)
		{
			_ = proxy ?? throw Exceptions.ArgumentNull(nameof(proxy));
			_ = configuration ?? throw Exceptions.ArgumentNull(nameof(configuration));
			_ = log ?? throw Exceptions.ArgumentNull(nameof(log));

			_ = configuration[RemoteEndpointKey] ?? throw Exceptions.InvalidOperation("'RemoteEndpoint' is not defined");
			_ = configuration[SpecLocationKey] ?? throw Exceptions.InvalidOperation("'SpecLocation' is not defined");

			this.proxy = proxy;
			this.log = log;
			this.remoteEndpoint = configuration[RemoteEndpointKey];
			this.specLocation = configuration[SpecLocationKey];
			log.LogInformation($"Forwarding to {remoteEndpoint}");
			log.LogInformation($"Spec located at {specLocation}");
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			if (ProxyEndpoints.Contains(context.Request.Path.Value))
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
				catch (AggregateException agex)
				{
					if (agex.InnerException != null && agex.InnerException is SpecNotFoundException)
					{
						SpecNotFoundException spex = (SpecNotFoundException)agex.InnerException;
						log.LogError(agex, $"Failed to retrieve spec - please recheck SpecLocation and SpecToken.{Environment.NewLine}StatusCode: {(int)spex.Response.StatusCode}{Environment.NewLine}ReasonPhrase: {spex.Response.ReasonPhrase}");
					}
					else
					{
						//an example edge case would be a non-existent domain like https://raw.fakegithubusercontent.com/HylandSoftware/Hot-Potato/master/test/RawPotatoSpec.yaml
						log.LogError(agex, "Exception thrown from an async call");
					}
					//we'll probably want to set to context.Reponse status code in the future,
					//but for now trying to set it here will cause a "Response already started")
				}
				catch (Exception e)
				{
					//handle unknown exceptions
					this.log.LogError(e, "Failed to forward request");
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				}
			}
		}
	}
}