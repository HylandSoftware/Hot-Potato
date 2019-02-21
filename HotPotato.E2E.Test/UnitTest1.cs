using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HotPotato.AspNetCore.Host;
using HttpMock;
using System;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.E2E.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            //Setting up mock server to hit
            const string expected = "ValidResponse";

            var _stubHttp = HttpMockRepository.At("http://localhost:9191");

            _stubHttp.Stub(x => x.Get("/endpoint"))
                .Return(expected)
                .OK();

            //Setting up proxy host
            var host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true);
                        //.AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddDebug();
                    }
                })
                .UseKestrel((options) =>
                {
                    options.AddServerHeader = false;
                })
                .UseUrls("http://0.0.0.0:3232")
                .UseStartup<Startup>()
                .Build();

            host.Start();

            //Setting up Http Client
            HttpClient client = new HttpClient();
            HttpMethod method = new HttpMethod("get");
            HttpRequestMessage req = new HttpRequestMessage(method, "http://localhost:3232/endpoint");
            HttpResponseMessage res = await client.SendAsync(req);


            
            //asserts
            //ok
            //length
            //string
            
        }
    }
}
