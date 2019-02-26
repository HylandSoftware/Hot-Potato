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

            using (var _stubHttp = HttpMockRepository.At(ApiLocation))
            {
                _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(expected)
                .OK();

                using (var host = SetupWebHost())
                {
                    host.Start();

                    using (HttpClient client = new HttpClient())
                    {
                        HttpMethod method = new HttpMethod(GetMethodCall);

                        using (HttpRequestMessage req = new HttpRequestMessage(method, ProxyEndpoint))
                        {
                            HttpResponseMessage res = await client.SendAsync(req);

                            //Asserts
                            Assert.Equal(OKResponseMessage, res.ReasonPhrase);
                            Assert.Equal(13, res.Content.Headers.ContentLength);
                            Assert.Equal(PlainTextContentType, res.Content.Headers.ContentType.MediaType);
                            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
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

            using (var _stubHttp = HttpMockRepository.At(ApiLocation))
            {
                _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(json)
                .AsContentType(ApplicationJsonContentType)
                .OK();

                using (var host = SetupWebHost())
                {
                    host.Start();

                    using (HttpClient client = new HttpClient())
                    {
                        HttpMethod method = new HttpMethod(GetMethodCall);

                        using (HttpRequestMessage req = new HttpRequestMessage(method, ProxyEndpoint))
                        {

                            HttpResponseMessage res = await client.SendAsync(req);

                            //Asserts
                            Assert.Equal(OKResponseMessage, res.ReasonPhrase);
                            Assert.Equal(ApplicationJsonContentType, res.Content.Headers.ContentType.MediaType);
                            Assert.Equal(json, res.Content.ReadAsStringAsync().Result); 
                        } 
                    }
                }
                
            }  
        }

        [Fact]
        public async Task HotPotato_Should_Return_404_Error()
        {
            //Setting up mock server to hit
            const string expected = NotFoundResponseMessage;

            using (var _stubHttp = HttpMockRepository.At(ApiLocation))
            {
                _stubHttp.Stub(x => x.Get(Endpoint))
                .Return(expected)
                .NotFound();

                using (var host = SetupWebHost())
                {
                    host.Start();

                    using (HttpClient client = new HttpClient())
                    {
                        HttpMethod method = new HttpMethod(GetMethodCall);

                        using (HttpRequestMessage req = new HttpRequestMessage(method, ProxyEndpoint))
                        {

                            HttpResponseMessage res = await client.SendAsync(req);

                            //Asserts
                            Assert.Equal(NotFoundResponseMessage, res.ReasonPhrase);
                            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
        }

        [Fact]        
        public async Task HotPotato_Should_Return_500_Error()
        {
            //Setting up mock server to hit
            const string expected = InternalServerErrorResponseMessage;

            using (var _stubHttp = HttpMockRepository.At(ApiLocation))
            {
                _stubHttp.Stub(x => x.Get(Endpoint))
               .Return(expected)
               .WithStatus(HttpStatusCode.InternalServerError);

                using (var host = SetupWebHost())
                {
                    host.Start();

                    //Setting up Http Client
                    using (HttpClient client = new HttpClient())
                    {
                        HttpMethod method = new HttpMethod(GetMethodCall);
                        
                        using (HttpRequestMessage req = new HttpRequestMessage(method, ProxyEndpoint))
                        {

                            HttpResponseMessage res = await client.SendAsync(req);

                            //Asserts
                            Assert.Equal(InternalServerErrorResponseMessage, res.ReasonPhrase);
                            Assert.Equal(expected, res.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
        }

        private IWebHost SetupWebHost()
        {
            //Setting up proxy host
            var host = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                    //.AddJsonFile("appsettings.json", optional: true);
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
                .UseSetting("RemoteEndpoint", ApiLocation)
                .UseUrls("http://0.0.0.0:3232")
                .UseStartup<Startup>()
                .Build();

            return host;
        }
    }
}
