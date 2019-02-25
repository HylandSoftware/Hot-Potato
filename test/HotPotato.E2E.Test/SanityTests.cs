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
        private HttpResponseMessage res;
        private const string ApiLocation = "http://localhost:9191";
        private const string Endpoint = "/endpoint";
        private const string ProxyEndpoint = "http://localhost:3232/endpoint";
        private const string GetMethodCall = "GET";
        private const string OKResponseMessage = "OK";
        private const string NotFoundResponseMessage = "Not Found";
        private const string InternalServerErrorResponseMessage = "Internal Server Error";
        private const string PlainTextContentType = "text/plain";
        private const string ApplicationJsonContentType = "application/json";

        [Fact]
        public async Task HotPotato_Should_Return_OK_And_A_String()
        {
            //Setting up mock server to hit
            const string expected = "ValidResponse";

            var _stubHttp = HttpMockRepository.At(ApiLocation);

            _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(expected)
                .OK();

            SetupWebHost();
            host.Start();

            await SetupClientAsync();

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal(OKResponseMessage, res.ReasonPhrase);
            Assert.Equal(13, res.Content.Headers.ContentLength);
            Assert.Equal(PlainTextContentType, res.Content.Headers.ContentType.MediaType);
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

            var _stubHttp = HttpMockRepository.At(ApiLocation);

            _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(json)
                .AsContentType(ApplicationJsonContentType)
                .OK();

            SetupWebHost();
            host.Start();

            await SetupClientAsync();

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal(OKResponseMessage, res.ReasonPhrase);
            Assert.Equal(ApplicationJsonContentType, res.Content.Headers.ContentType.MediaType);
            Assert.Equal(json, res.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task HotPotato_Should_Return_404_Error()
        {
            //Setting up mock server to hit
            const string expected = NotFoundResponseMessage;

            var _stubHttp = HttpMockRepository.At(ApiLocation);

            _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(expected)
                .NotFound();

            SetupWebHost();
            host.Start();

            //Setting up Http Client
            await SetupClientAsync();

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal(NotFoundResponseMessage, res.ReasonPhrase);
            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
        }

        [Fact]        
        public async Task HotPotato_Should_Return_500_Error()
        {
            //Setting up mock server to hit
            const string expected = InternalServerErrorResponseMessage;

            var _stubHttp = HttpMockRepository.At(ApiLocation);

            _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(expected)
                .WithStatus(HttpStatusCode.InternalServerError);

            SetupWebHost();
            host.Start();

            //Setting up Http Client
            await SetupClientAsync();

            //Dispose host so the port can be used by other tests
            host.Dispose();

            //Asserts
            Assert.Equal(InternalServerErrorResponseMessage, res.ReasonPhrase);
            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
        }

        private async Task SetupClientAsync()
        {
            //Setting up Http Client
            HttpClient client = new HttpClient();
            HttpMethod method = new HttpMethod(GetMethodCall);
            HttpRequestMessage req = new HttpRequestMessage(method, ProxyEndpoint);
            HttpResponseMessage res = await client.SendAsync(req);

            this.res = res;
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
