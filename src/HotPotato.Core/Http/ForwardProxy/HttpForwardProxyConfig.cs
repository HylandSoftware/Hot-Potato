using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Core.Http.ForwardProxy
{
	public class HttpForwardProxyConfig
	{
		public bool Enabled { get; set; }
		public string ProxyAddress { get; set; }
		public bool BypassOnLocal { get; set; }
	}
}
