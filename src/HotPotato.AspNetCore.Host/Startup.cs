using HotPotato.Core.Processor;
using HotPotato.Core.Proxy;
using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.AspNetCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using HotPotato.OpenApi.Processor;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using HotPotato.OpenApi.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HotPotato.AspNetCore.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public ILogger Log { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> log)
        {
            _ = configuration ?? throw new ArgumentException(nameof(configuration));
            _ = log ?? throw new ArgumentException(nameof(log));

            Configuration = configuration;
            Log = log;
        }

        public void Configure(ILoggerFactory loggerFactory, IApplicationBuilder builder, IHostingEnvironment env)
        {
            builder.UseResponseBuffering();
            builder.UseMiddleware<HotPotatoMiddleware>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProxy, HotPotato.Core.Proxy.Default.Proxy>();
            services.AddScoped<IHttpClient, HttpClient>();
            services.AddHttpClient<IHttpClient, HttpClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["RemoteEndpoint"]);
            });
            services.AddSingleton<ISpecificationProvider, SpecificationProvider>();
            services.AddSingleton<IValidationProvider, ValidationProvider>();
            services.AddSingleton<IResultCollector, ResultCollector>();

            services.AddTransient<PathValidator>();
            services.AddTransient<MethodValidator>();
            services.AddTransient<StatusCodeValidator>();
            services.AddTransient<BodyValidator>();
            services.AddTransient<HeaderValidator>();

            services.AddTransient<Func<string, IValidator>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "path":
                        return serviceProvider.GetService<PathValidator>();
                    case "method":
                        return serviceProvider.GetService<MethodValidator>();
                    case "status":
                        return serviceProvider.GetService<StatusCodeValidator>();
                    case "body":
                        return serviceProvider.GetService<BodyValidator>();
                    case "header":
                        return serviceProvider.GetService<HeaderValidator>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddTransient<IProcessor, Processor>();

        }
    }
}
