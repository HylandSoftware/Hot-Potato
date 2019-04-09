using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using System.Net;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationBuilder
    {

        private Validator val { get; }
        private string Path { get; set; }
        private HttpMethod Method { get; set; }
        private HttpStatusCode StatusCode { get; set; }
        private string Body { get; set; }
        private HttpHeaders Headers { get; set; }

        public ValidationBuilder(IResultCollector resColl, ISpecificationProvider specPro)
        {
            val = new Validator(resColl, specPro);
        }

        public ValidationBuilder WithPath(string path)
        {
            Path = path;
            return this;
        }
        
        public ValidationBuilder WithMethod(HttpMethod method)
        {
            Method = method;
            return this;
        }
        
        public ValidationBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public ValidationBuilder WithBody(string body, string contentType)
        {
            Body = body;
            return this;
        }

        public ValidationBuilder WithHeaders(HttpHeaders headers)
        {
            Headers = headers;
            return this;
        }

        public Validator Build()
        {
            val.pathVal = new PathValidator(Path);
            val.methodVal = new MethodValidator(Method);
            val.statusCodeVal = new StatusCodeValidator(StatusCode, Body);
            val.bodyVal = new BodyValidator(Body);
            val.headerVal = new HeaderValidator(Headers);
            return val;
        }
    }
}
