using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using System.Net;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationBuilder
    {

        private readonly Validator val;

        public ValidationBuilder(IResultCollector resColl, ISpecificationProvider specPro)
        {
            val = new Validator(resColl, specPro);
        }

        public ValidationBuilder WithPath(string path)
        {
            val.pathVal = new PathValidator(path);
            return this;
        }
        
        public ValidationBuilder WithMethod(HttpMethod method)
        {
            val.methodVal = new MethodValidator(method);
            return this;
        }
        
        public ValidationBuilder WithStatusCode(HttpStatusCode statusCode, string body)
        {
            val.statusCodeVal = new StatusCodeValidator(statusCode, body);
            return this;
        }

        public ValidationBuilder WithBody(string body, string contentType)
        {
            val.bodyVal = new BodyValidator(body, contentType);
            return this;
        }

        public ValidationBuilder WithHeaders(HttpHeaders headers)
        {
            val.headerVal = new HeaderValidator(headers);
            return this;
        }

        public Validator Build()
        {
            return val;
        }
    }
}
