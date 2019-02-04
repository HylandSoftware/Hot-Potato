using HotPotato.Http.Default;
using HotPotato.Models;
using System;
using System.Net;
using System.Net.Http;
using Moq;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Locators.NSwag
{
    public class StatusCodeLocatorTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/documents/life-cycles/";
        [Fact]
        public void StatCodeLocator_RespMatchesOp()
        {
            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint));
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            SwaggerOperation swagOp = new SwaggerOperation();
            swagOp.Responses.Add("200", new SwaggerResponse());

            StatusCodeLocator statLoc = new StatusCodeLocator();
            SwaggerResponse subject = statLoc.Locate(testPair, swagOp);
            Assert.NotNull(subject);
        }
    }

}