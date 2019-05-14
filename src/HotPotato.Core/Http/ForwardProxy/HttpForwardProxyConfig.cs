using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Core.Http.ForwardProxy
{
    public class HttpForwardProxyConfig
    {
        public bool enabled { get; set; }
        public string proxyAddress { get; set; }
        public bool bypassOnLocal { get; set; }
    }
}
