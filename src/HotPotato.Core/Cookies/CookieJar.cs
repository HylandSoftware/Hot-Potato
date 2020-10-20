using Microsoft.Extensions.Configuration;
using System.Net;
using System;

namespace HotPotato.Core.Cookies
{
	public class CookieJar : ICookieJar
	{
		private const string RemoteEndpointKey = "RemoteEndpoint";
		private readonly string remoteEndpoint;
		
		public System.Net.CookieContainer Cookies { get; }

		public CookieJar(IConfiguration configuration)
		{
			_ = configuration ?? throw Exceptions.ArgumentNull(nameof(configuration));
			_ = configuration[RemoteEndpointKey] ?? throw Exceptions.InvalidOperation("'RemoteEndpoint' is not defined");
			this.Cookies = new System.Net.CookieContainer();
			this.remoteEndpoint = configuration[RemoteEndpointKey];
		}

		public void ExpireCookies()
		{
			if (Cookies.Count > 0)
			{
				CookieCollection cookies = Cookies.GetCookies(new Uri(remoteEndpoint));
				foreach (Cookie cookie in cookies)
				{
					cookie.Expired = true;
				}
			}
		}
	}
}
