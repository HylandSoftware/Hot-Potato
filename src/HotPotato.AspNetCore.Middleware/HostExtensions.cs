using HotPotato.Core.Cookies;
using HotPotato.Core.Http;
using HotPotato.Core.Processor;
using HotPotato.Core.Proxy;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Processor;
using HotPotato.OpenApi.SpecificationProvider;
using Microsoft.Extensions.DependencyInjection;

namespace HotPotato.AspNetCore.Middleware
{
    public static class HostExtensions
    {
        /// <summary>
        /// Configure all the necessary services needed to use HotPotato's Middleware with an external host
        /// </summary>
        /// <param name="services">The client from the external host to be injected into the service collection</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMiddlewareServices(this IServiceCollection services, Core.Http.Default.HotPotatoClient client = null)
        {
            services.AddScoped<IProxy, HotPotato.Core.Proxy.Default.Proxy>();
            if (client != null)
            {
                services.AddSingleton<IHotPotatoClient>(client);
            }
            else
            {
                services.AddScoped<IHotPotatoClient, HotPotato.Core.Http.Default.HotPotatoClient>();
                services.AddHttpClient<IHotPotatoClient, HotPotato.Core.Http.Default.HotPotatoClient>();
            }
            services.AddSingleton<ICookieJar, CookieJar>();
            services.AddSingleton<ISpecificationProvider, SpecificationProvider>();
            services.AddSingleton<IResultCollector, ResultCollector>();
            services.AddTransient<IProcessor, Processor>();
            return services;
        }
    }
}
