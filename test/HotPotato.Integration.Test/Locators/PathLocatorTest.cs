
using HotPotato.Http.Default;
using static HotPotato.IntegrationTestMethods;
using HotPotato.Models;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HotPotato.OpenApi.Locators.NSwag
{
    public class PathLocatorTest
    {
        [Fact]
        public void PathLocator_LocatesWithParam()
        {
            string endpointWithPar = "http://api.docs.hyland.io/keyword/keyword-type-groups/48732/keyword-types";
            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithPar));
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            string path = Path.Combine(Environment.CurrentDirectory, SpecPath("specs/keyword/"), "specification.yaml");
            Task<SwaggerDocument> swagTask = FromFileAsync(path);
            SwaggerDocument swagDoc = swagTask.Result;

            PathLocator subject = new PathLocator();
            SwaggerPathItem result = subject.Locate(testPair, swagDoc);
            Assert.NotNull(result);
        }

        [Fact]
        public void PathLocator_LocatesWithoutParam()
        {
            string endpointWithoutPar = "https://api.hyland.com/workflow/life-cycles";
            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithoutPar));
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            string path = Path.Combine(Environment.CurrentDirectory, SpecPath("specs/workflow/"), "specification.yaml");
            Task<SwaggerDocument> swagTask = FromFileAsync(path);
            SwaggerDocument swagDoc = swagTask.Result;

            PathLocator subject = new PathLocator();
            SwaggerPathItem result = subject.Locate(testPair, swagDoc);
            Assert.NotNull(result);
        }

    }

}
