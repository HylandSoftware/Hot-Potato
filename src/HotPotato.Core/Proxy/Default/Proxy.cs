using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HotPotato.Core.Proxy.Default
{
	public class Proxy : IProxy
	{

		private IHotPotatoClient Client { get; }
		private ILogger Logger { get; }
		private IProcessor Processor { get; }

		public Proxy(IHotPotatoClient client, ILogger<Proxy> logger, IProcessor processor)
		{
			_ = client ?? throw Exceptions.ArgumentNull(nameof(client));
			_ = logger ?? throw Exceptions.ArgumentNull(nameof(logger));
			_ = processor ?? throw Exceptions.ArgumentNull(nameof(processor));

			this.Client = client;
			this.Logger = logger;
			this.Processor = processor;
		}

		public async Task ProcessAsync(string remoteEndpoint, HttpRequest requestIn, HttpResponse responseOut)
		{
			using (IHotPotatoRequest request = await requestIn.ToProxyRequest(remoteEndpoint))
			{
				IHotPotatoResponse response = await this.Client.SendAsync(request);
				await response.ToProxyResponseAsync(responseOut);
				using (HttpPair pair = new HttpPair(request, response))
				{
					this.Processor.Process(pair);
				}
			}
		}
	}
}
