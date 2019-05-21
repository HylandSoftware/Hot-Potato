using HotPotato.OpenApi.Models;
using Newtonsoft.Json;
using Xunit;

namespace HotPotato.OpenApi.Results
{
    public class FailResultTest
    {
        private readonly string[] expectedKeys = { "Path", "Method", "StatusCode", "State", "Reason", "ValidationErrors" };

        private const string path = "/path";
        private const string method = "trace";
        private const int statusCode = 200;
        private const Reason reason = Reason.InvalidBody;

        [Fact]
        public void PassResult_SerializesWithExpectedKeys()
        {
            FailResult subject = new FailResult(path, method, statusCode, reason, null);

            string result = JsonConvert.SerializeObject(subject);

            foreach (string expectedKey in expectedKeys)
            {
                Assert.Contains(expectedKey, result);
            }
        }
    }
}
