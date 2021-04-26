using HotPotato.Api.Models;
using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HotPotato.TestServ.Test
{
    public class RawPotatoTest : IClassFixture<TestFixture<Api.Startup>>
    {
        private Core.Http.Default.HttpClient client;
        private List<Result> results;

        private readonly Order paperOrder = new Order()
        {
            Id = 5,
            Price = 10.00,
            Items = new List<Item>()
            {
                new Item()
                {
                    ItemId = 1,
                    Name = "Paper",
                    Price = 10.00
                }
            }
        };

        public RawPotatoTest(TestFixture<Api.Startup> fixture)
        {
            client = fixture.Client;
            results = fixture.Results;
        }

        [Theory]
        [InlineData("http://localhost:3232", "GET", 200)]
        [InlineData("http://localhost:3232/order", "GET", 200)]
        [InlineData("http://localhost:3232/order", "POST", 201, true)]
        [InlineData("http://localhost:3232/order/3", "PUT", 204, true)]
        [InlineData("http://localhost:3232/order/1/items/3", "DELETE", 204)]
        [InlineData("http://localhost:3232/order/2", "OPTIONS", 200)]
        [InlineData("http://localhost:3232/order/4", "GET", 200)]
        public async Task HotPotato_Should_Process_RawPotato_HappyPaths(string path, string methodString, int expectedStatusCode, bool hasRequestBody = false)
        {
            HttpMethod method = new HttpMethod(methodString);
            Uri pathUri = new Uri(path);

            using (HttpRequest req = new HttpRequest(method, pathUri))
            {
                if (hasRequestBody)
                {
                    string paperOrderBody = JsonConvert.SerializeObject(paperOrder);
                    req.SetContent(paperOrderBody);
                }
                await client.SendAsync(req);

                Result result = results.ElementAt(0);

                Assert.Equal(methodString, result.Method, ignoreCase: true);
                Assert.Equal(pathUri.AbsolutePath, result.Path);
                Assert.Equal(State.Pass, result.State);
                Assert.Equal(expectedStatusCode, result.StatusCode);

                results.Clear();
            }
        }
    }
}
