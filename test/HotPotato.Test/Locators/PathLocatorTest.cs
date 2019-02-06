using HotPotato.Http.Default;
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
        private static char s = Path.DirectorySeparatorChar;
        [Fact]
        public void PathLocator_LocatesWithParam()
        {
            string endpointWithPar = "http://api.docs.hyland.io/keyword/keyword-type-groups/48732/keyword-types";
            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithPar));
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            string path = Path.Combine(Environment.CurrentDirectory, String.Format("specs{0}keyword{0}", s), "specification.yaml");
            Task<SwaggerDocument> swagTask = FromFileAsync(path);
            SwaggerDocument swagDoc = swagTask.Result;

            PathLocator pathLoc = new PathLocator();
            SwaggerPathItem subject = pathLoc.Locate(testPair, swagDoc);
            Assert.NotNull(subject);
        }

        [Fact]
        public void PathLocator_LocatesWithoutParam()
        {
            string endpointWithoutPar = "https://api.hyland.com/workflow/life-cycles";
            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithoutPar));
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);
            HttpPair testPair = new HttpPair(testRequest, testResponse);

            string path = Path.Combine(Environment.CurrentDirectory, String.Format("specs{0}workflow{0}", s), "specification.yaml");
            Task<SwaggerDocument> swagTask = FromFileAsync(path);
            SwaggerDocument swagDoc = swagTask.Result;

            PathLocator pathLoc = new PathLocator();
            SwaggerPathItem subject = pathLoc.Locate(testPair, swagDoc);
            Assert.NotNull(subject);
        }

    }

}
