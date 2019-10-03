using HotPotato.Core.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using Xunit;

namespace HotPotato.OpenApi.Models
{
    public class PassResultWithCustomHeadersTest
    {
        private readonly string[] expectedKeys = { "Custom", "State", "Path", "Method", "StatusCode" };

        private const string path = "/path";
        private const string method = "trace";
        private const int statusCode = 200;

        private const string AValidCustomHeaderKey = "X-HP-Custom-Header-Key";
        private const string AValidCustomHeaderValue = "Custom-Header-Value";

        [Fact]
        public void PassResultWithCustomHeaders_SerializesWithCustomHeaderFirst()
        {
            var customHeaders = new HttpHeaders();
            customHeaders.Add(AValidCustomHeaderKey, AValidCustomHeaderValue);

            PassResultWithCustomHeaders subject = new PassResultWithCustomHeaders(path, method, statusCode, customHeaders);

            JObject result = JObject.FromObject(subject);
            JToken property = result.First;

            foreach (string expectedKey in expectedKeys)
            {
                Assert.Equal(expectedKey, property.Path);
                property = property.Next;
            }
        }
    }
}
