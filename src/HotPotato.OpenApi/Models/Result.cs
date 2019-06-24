
namespace HotPotato.OpenApi.Models
{
    public abstract class Result
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public virtual State State { get; protected set; }
        public virtual string Path { get; protected set; }
        public virtual string Method { get; protected set; }
        public virtual int StatusCode { get; protected set; }
    }
}
