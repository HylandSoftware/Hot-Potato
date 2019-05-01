using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using System.Net;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationBuilder
    {

        private ValidationStrategy val { get; set; }
        private string Path { get; set; }
        private HttpMethod Method { get; set; }
        private HttpStatusCode StatusCode { get; set; }
        private string Body { get; set; }
        private HttpHeaders Headers { get; set; }

        private IResultCollector ResultCollector { get; }
        private ISpecificationProvider SpecificationProvider { get; }

        public ValidationBuilder(IResultCollector resColl, ISpecificationProvider specPro)
        {
            ResultCollector = resColl;
            SpecificationProvider = specPro;
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

        public ValidationBuilder WithBody(string body)
        {
            Body = body;
            return this;
        }

        public ValidationBuilder WithHeaders(HttpHeaders headers)
        {
            Headers = headers;
            return this;
        }

        public IValidationStrategy Build()
        {
            val = new ValidationStrategy(ResultCollector, SpecificationProvider);
            val.PathValidator = new PathValidator(Path);
            val.MethodValidator = new MethodValidator(Method);
            val.StatusCodeValidator = new StatusCodeValidator(StatusCode, Body);
            val.BodyValidator = new BodyValidator(Body);
            val.HeaderValidator = new HeaderValidator(Headers);
            return val;
        }
    }
}
