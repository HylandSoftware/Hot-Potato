
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace HotPotato.OpenApi.Models
{
    public class PassResultTest
    {
        private readonly string[] expectedKeys = { "State", "Path", "Method", "StatusCode" };
        private readonly string[] failKeys = { "Reason", "ValidationErrors" };

        private const string path = "/path";
        private const string method = "trace";
        private const int statusCode = 200;

        [Fact]
        public void PassResult_SerializesWithExpectedKeysInOrder()
        {
            PassResult subject = new PassResult(path, method, statusCode);

            JObject result = JObject.FromObject(subject);
            JToken property = result.First;

            foreach (string expectedKey in expectedKeys)
            {
                Assert.Equal(expectedKey, property.Path);
                property = property.Next;
            }
        }

        [Fact]
        public void PassResult_SerializesWithoutFailKeys()
        {
            PassResult subject = new PassResult(path, method, statusCode);

            string result = JsonConvert.SerializeObject(subject);

            foreach (string failKey in failKeys)
            {
                Assert.DoesNotContain(failKey, result);
            }
        }
    }
}
