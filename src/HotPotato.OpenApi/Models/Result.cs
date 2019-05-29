
namespace HotPotato.OpenApi.Models
{
    public abstract class Result
    {
        public string Path { get; protected set; }
        public string Method { get; protected set; }
        public int StatusCode { get; protected set; }
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public State State { get; protected set; }       
    }
}
