
using Newtonsoft.Json;
using Xunit;

namespace HotPotato.OpenApi.Results
{
    public class PassResultTest
    {
        private readonly string[] expectedKeys = { "Path", "Method", "StatusCode", "State" };
        private readonly string[] failKeys = { "Reason", "ValidationErrors" };

        private const string path = "/path";
        private const string method = "trace";
        private const int statusCode = 200;

        [Fact]
        public void PassResult_SerializesWithExpectedKeys()
        {
            PassResult subject = new PassResult(path, method, statusCode);

            string result = JsonConvert.SerializeObject(subject);

            foreach (string expectedKey in expectedKeys)
            {
                Assert.Contains(expectedKey, result);
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
