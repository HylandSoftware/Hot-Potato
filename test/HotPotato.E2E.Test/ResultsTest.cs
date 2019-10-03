﻿using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace HotPotato.E2E.Test
{
    [Collection("Host")]
    public class ResultsTest
    {
        private IWebHost host;

        private const string ApiLocation = "http://localhost:5000";
        private const string Endpoint = "/endpoint";
        private const string ProxyEndpoint = "http://localhost:3232/endpoint";
        private const string GetMethodCall = "GET";
        private const string OKResponseMessage = "OK";
        private const string PlainTextContentType = "text/plain";
        private const string ContentType = "Content-Type";

        private const string ACustomHeaderKey = "X-HP-A-Custom-Header-Key";
        private const string ACustomHeaderValue = "A-Custom-Header-Value";

        public ResultsTest(HostFixture fixture)
        {
            host = fixture.host;
        }


        [Fact]
        public async Task HotPotato_Should_Set_Respective_Custom_Headers()
        {
            var servicePro = host.Services;

            //Setting up mock server to hit
            const string expected = "ValidResponse";

            using (var server = FluentMockServer.Start(ApiLocation))
            {
                server
                    .Given(
                        Request.Create()
                            .WithPath(Endpoint)
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader(ContentType, PlainTextContentType)
                            .WithBody(expected)
                    );

                Core.Http.Default.HttpClient client = (Core.Http.Default.HttpClient)servicePro.GetService<IHttpClient>();

                HttpMethod method = new HttpMethod(GetMethodCall);

                IResultCollector resultCollector = servicePro.GetService<IResultCollector>();
                resultCollector.Results.Clear();

                using (HttpRequest req = new HttpRequest(method, new System.Uri(ProxyEndpoint)))
                {
                    req.HttpHeaders.Add(ACustomHeaderKey, ACustomHeaderValue);
                    IHttpResponse res = await client.SendAsync(req);
                }

                //second request to make sure custom header doesn't linger
                using (HttpRequest req = new HttpRequest(method, new System.Uri(ProxyEndpoint)))
                {
                    IHttpResponse res = await client.SendAsync(req);
                }

                FailResultWithCustomHeaders resultWithCustomHeaders = (FailResultWithCustomHeaders)resultCollector.Results.ElementAt(0);
                Result resultWithoutCustomHeaders = resultCollector.Results.ElementAt(1);

                Assert.True(resultWithCustomHeaders.Custom.ContainsKey(ACustomHeaderKey));
                Assert.False(resultWithoutCustomHeaders is FailResultWithCustomHeaders);
            }
        }
    }
}
