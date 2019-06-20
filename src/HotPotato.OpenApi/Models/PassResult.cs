using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HotPotato.OpenApi.Models
{
    public class PassResult : Result
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public override State State => State.Pass;
        public PassResult(string path, string method, int statusCode)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
        }
    }
}
