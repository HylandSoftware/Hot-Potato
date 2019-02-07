using HotPotato.Http.Default;
using HotPotato.OpenApi.Locators.NSwag;
using HotPotato.Models;
using HotPotato.Validators;
//using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using NJsonSchema;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using Xunit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.Http.Default
{
    //M:\git\specifications\specs\ccm
    //M:\git\specifications\specs\cv
    //M:\git\specifications\specs\deficiencies
    //M:\git\specifications\specs\document
    //M:\git\specifications\specs\keyword
    //M:\git\specifications\specs\rdds\configurationservice
    //M:\git\specifications\specs\rdds\messagestorageservice
    //M:\git\specifications\specs\rdds\onrampservice
    //M:\git\specifications\specs\workflow

    public class SpecTest
    {
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles";
        [Fact]
        public void LocatorReturnsValidSchema()
        {
            //AValidEndpoint = path
            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint));
            HttpResponse testResponse = new HttpResponse(HttpStatusCode.OK, null);
            HttpPair testPair = new HttpPair(testRequest, testResponse);
            string path = "M:\\git\\specifications\\specs\\workflow\\specification.yaml";
            Task<SwaggerDocument> derp = FromFileAsync(path);
            var swagDoc = derp.Result;
            Locator docLoc = new Locator(swagDoc, new PathLocator(), new MethodLocator(), new StatusCodeLocator());
            Tuple<IBodyValidator, IHeaderValidator> tup = docLoc.GetValidator(testPair);
            Assert.Null(tup);
            //HttpResponse testResponse = new HttpResponse();

        }
    }
}
