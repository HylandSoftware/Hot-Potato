using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HotPotato.AspNetCore.Host;
using HttpMock;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace HotPotato.E2E.Test
{
    public class SanityTests
    {

        private IWebHost host;

        [Fact]
        public async Task HotPotato_Should_Return_OK_And_A_String()
        {
            //Setting up mock server to hit
            const string expected = "ValidResponse";

            var _stubHttp = HttpMockRepository.At("http://localhost:9191");

            _stubHttp.Stub(x => x.Get("/endpoint"))
                .Return(expected)
                .OK();

            SetupWebHost();
            host.Start();

            //Setting up Http Client
            HttpClient client = new HttpClient();
            HttpMethod method = new HttpMethod("GET");
            HttpRequestMessage req = new HttpRequestMessage(method, "http://localhost:3232/endpoint");
            HttpResponseMessage res = await client.SendAsync(req);

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal("OK", res.ReasonPhrase);
            Assert.Equal(13, res.Content.Headers.ContentLength);
            Assert.Equal("text/plain", res.Content.Headers.ContentType.MediaType);
            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task HotPotato_Should_Return_OK_And_A_JSON_Object()
        {
            //Setting up mock server to hit
            string json = @"{
                'Email': 'james@example.com',
                'Active': true,
                'CreatedDate': '2013-01-20T00:00:00Z',
                'Roles': [
                    'User',
                    'Admin'
                ]}";

            var _stubHttp = HttpMockRepository.At("http://localhost:9191");

            _stubHttp.Stub(x => x.Get("/endpoint"))
                .Return(json)
                .AsContentType("application/json")
                .OK();

            SetupWebHost();
            host.Start();

            //Setting up Http Client
            HttpClient client = new HttpClient();
            HttpMethod method = new HttpMethod("GET");
            HttpRequestMessage req = new HttpRequestMessage(method, "http://localhost:3232/endpoint");
            HttpResponseMessage res = await client.SendAsync(req);

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal("OK", res.ReasonPhrase);
            Assert.Equal("application/json", res.Content.Headers.ContentType.MediaType);
            Assert.Equal(json, res.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task HotPotato_Should_Return_404_Error()
        {
            //Setting up mock server to hit
            const string expected = "Not Found";

            var _stubHttp = HttpMockRepository.At("http://localhost:9191");

            _stubHttp.Stub(x => x.Get("/endpoint"))
                .Return(expected)
                .NotFound();

            SetupWebHost();
            host.Start();

            //Setting up Http Client
            HttpClient client = new HttpClient();
            HttpMethod method = new HttpMethod("GET");
            HttpRequestMessage req = new HttpRequestMessage(method, "http://localhost:3232/endpoint");
            HttpResponseMessage res = await client.SendAsync(req);

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal("Not Found", res.ReasonPhrase);
            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
        }

        [Fact]        
        public async Task HotPotato_Should_Return_500_Error()
        {
            //Setting up mock server to hit
            const string expected = "InternalServerError";

            var _stubHttp = HttpMockRepository.At("http://localhost:9191");

            _stubHttp.Stub(x => x.Get("/endpoint"))
                .Return(expected)
                .WithStatus(HttpStatusCode.InternalServerError);

            SetupWebHost();
            host.Start();

            //Setting up Http Client
            HttpClient client = new HttpClient();
            HttpMethod method = new HttpMethod("GET");
            HttpRequestMessage req = new HttpRequestMessage(method, "http://localhost:3232/endpoint");
            HttpResponseMessage res = await client.SendAsync(req);

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal("Internal Server Error", res.ReasonPhrase);
            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
        }

        private void SetupWebHost()
        {
            //Setting up proxy host
            var host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true);
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

            this.host = host;
        }
    }
}
