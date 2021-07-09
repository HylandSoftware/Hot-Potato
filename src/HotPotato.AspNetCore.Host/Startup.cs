using HotPotato.AspNetCore.Middleware;
using HotPotato.Core;
using HotPotato.Core.Cookies;
using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Http.ForwardProxy;
using HotPotato.Core.Processor;
using HotPotato.Core.Proxy;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Processor;
using HotPotato.OpenApi.SpecificationProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;

namespace HotPotato.AspNetCore.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public ILogger Log { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> log)
        {
            _ = configuration ?? throw Exceptions.ArgumentNull(nameof(configuration));
            _ = log ?? throw Exceptions.ArgumentNull(nameof(log));

            Configuration = configuration;
            Log = log;
        }

        public void Configure(ILoggerFactory loggerFactory, IApplicationBuilder builder, IHostingEnvironment env)
        {
            builder.UseMiddleware<HotPotatoMiddleware>();
            builder.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            bool ignoreClientCertificateValidationErrors = Configuration.GetSection("HttpClientSettings").GetValue<bool>("IgnoreClientHttpsCertificateValidationErrors");
            LogTlsValidationSetting(ignoreClientCertificateValidationErrors);

            services.AddScoped<IProxy, HotPotato.Core.Proxy.Default.Proxy>();
            services.AddScoped<IHotPotatoClient, HotPotatoClient>();
            services.AddMvcCore().AddJsonFormatters();
            services.AddSingleton<IWebProxy, Core.Http.ForwardProxy.Default.HttpForwardProxy>();
            services.AddSingleton(Configuration.GetSection("ForwardProxy").Get<HttpForwardProxyConfig>());
            services.AddSingleton<ICookieJar, CookieJar>();
            services.AddHttpClient<IHotPotatoClient, HotPotatoClient>()
            .ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                Proxy = sp.GetService<HttpForwardProxyConfig>().Enabled ? sp.GetService<IWebProxy>() : null,
                CookieContainer = sp.GetService<ICookieJar>().Cookies,
                ServerCertificateCustomValidationCallback = ignoreClientCertificateValidationErrors ?
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator :
                    null
            });

            services.AddSingleton<ISpecificationProvider, SpecificationProvider>();
            services.AddSingleton<IResultCollector, ResultCollector>();

            services.AddTransient<IProcessor, Processor>();
        }

        private void LogTlsValidationSetting(bool settingValue)
        {
            if (settingValue)
            {
                Log.LogWarning(
                    String.Format(
                        @"IgnoreClientCertificateValidation is set to TRUE! When Hot Potato sends requests to the remote API, SSL/TLS certificate validation errors will be ignored!"));
            }
            else
            {
                Log.LogInformation(
                    String.Format(
                        @"IgnoreClientCertificateValidation is set to false. When Hot Potato sends requests to the remote API, SSL/TLS certificate validation problems will cause critical application errors."));
            }
        }
    }
}