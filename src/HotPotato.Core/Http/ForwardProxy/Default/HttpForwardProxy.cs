using System;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HotPotato.Core.Http.ForwardProxy.Default
{
    public class HttpForwardProxy : IWebProxy
    {
        private ILogger Logger { get; set; }
        private Uri ProxyUri { get; set; }

        public HttpForwardProxy(HttpForwardProxyConfig configuration, ILogger<HttpForwardProxy> logger)
        {
            this.Logger = logger;
            //here you can load it from your custom config settings 
            try
            {
                this.ProxyUri = new Uri(configuration.ProxyAddress);
            }
            catch (ArgumentNullException)
            {
                logger.LogError("ForwardProxy is enabled but no proxyAddress was provided. Bypassing proxy.");
            }
            catch (UriFormatException)
            {
                logger.LogError("ForwardProxy is enabled but the value provided for proxyAddress cannot be convert to a URI. Bypassing proxy.");
            }
        }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return this.ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            //you can proxy all requests or implement bypass urls based on config settings
            return false;
        }
    }
}
