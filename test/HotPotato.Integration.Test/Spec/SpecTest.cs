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
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

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
        private const string AValidEndpoint = "https://api.hyland.com/workflow/life-cycles/48/";
        [Fact]
        public async void Locator_ReturnsValidBody()
        {
            object jsonData = new
            {
                id = "string",
                name = "string",
                smallIconId = "string"
            };

            string jsonString = JsonConvert.SerializeObject(jsonData);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpRequest testRequest = new HttpRequest(HttpMethod.Get, new Uri(AValidEndpoint));

            HttpResponseMessage testRespMsg = new HttpResponseMessage();
            testRespMsg.StatusCode = HttpStatusCode.OK;
            testRespMsg.Content = content;
            var testResponse = await testRespMsg.ConvertResponse();

            HttpPair testPair = new HttpPair(testRequest, testResponse);
            string path = "M:\\git\\specifications\\specs\\workflow\\specification.yaml";
            Task<SwaggerDocument> swagTask = FromFileAsync(path);
            SwaggerDocument swagDoc = swagTask.Result;

            Locator subject = new Locator(swagDoc, new PathLocator(), new MethodLocator(), new StatusCodeLocator());
            Tuple<IBodyValidator, IHeaderValidator> valTup = subject.GetValidator(testPair);
            Results.Result result = valTup.Item1.Validate(jsonString);
            Assert.Contains("is valid", result.Message);

        }
    }
}
