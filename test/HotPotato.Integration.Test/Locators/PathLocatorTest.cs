﻿using static HotPotato.IntegrationTestMethods;
using HotPotato.Core.Models;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HotPotato.Core.Http.Default;

namespace HotPotato.OpenApi.Locators.NSwag
{
    public class PathLocatorTest
    {
        [Fact]
        public void PathLocator_LocatesWithParam()
        {
            string endpointWithPar = "http://api.docs.hyland.io/keyword/keyword-type-groups/48732/keyword-types";
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithPar)))
            {
                HttpPair testPair = new HttpPair(testRequest, testResponse);

                string path = SpecPath("specs/keyword/", "specification.yaml");
                Task<SwaggerDocument> swagTask = FromFileAsync(path);
                SwaggerDocument swagDoc = swagTask.Result;

                PathLocator subject = new PathLocator();
                SwaggerPathItem result = subject.Locate(testPair, swagDoc);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void PathLocator_LocatesWithoutParam()
        {
            string endpointWithoutPar = "https://api.hyland.com/workflow/life-cycles";
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);

            using (HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(endpointWithoutPar)))
            {
                HttpPair testPair = new HttpPair(testRequest, testResponse);

                string path = SpecPath("specs/workflow/", "specification.yaml");
                Task<SwaggerDocument> swagTask = FromFileAsync(path);
                SwaggerDocument swagDoc = swagTask.Result;

                PathLocator subject = new PathLocator();
                SwaggerPathItem result = subject.Locate(testPair, swagDoc);
                Assert.NotNull(result);
            }
        }

    }

}
