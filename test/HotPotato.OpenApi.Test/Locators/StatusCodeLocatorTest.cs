using HotPotato.Core.Models;
using NSwag;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using HotPotato.Core.Http.Default;

namespace HotPotato.OpenApi.Locators.NSwag
{
    public class StatusCodeLocatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void StatCodeLocator_RespMatchesOp()
        {
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint)))
            {
                HttpPair testPair = new HttpPair(testRequest, testResponse);

                SwaggerOperation swagOp = new SwaggerOperation();
                swagOp.Responses.Add("200", new SwaggerResponse());

                StatusCodeLocator statLoc = new StatusCodeLocator();
                SwaggerResponse subject = statLoc.Locate(testPair, swagOp);
                Assert.NotNull(subject);
            }
        }
    }

}