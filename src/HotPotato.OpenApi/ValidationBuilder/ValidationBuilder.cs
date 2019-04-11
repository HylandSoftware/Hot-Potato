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
        private string path { get; set; }
        private HttpMethod method { get; set; }
        private HttpStatusCode statusCode { get; set; }
        private string body { get; set; }
        private string contentType { get; set; }
        private HttpHeaders headers { get; set; }

        public ValidationBuilder(IResultCollector resColl, ISpecificationProvider specPro)
        {
            val = new Validator(resColl, specPro);
        }

        public ValidationBuilder WithPath(string Path)
        {
            path = Path;
            return this;
        }
        
        public ValidationBuilder WithMethod(HttpMethod Method)
        {
            method = Method;
            return this;
        }
        
        public ValidationBuilder WithStatusCode(HttpStatusCode StatusCode)
        {
            statusCode = StatusCode;
            return this;
        }

        public ValidationBuilder WithBody(string Body, string ContentType)
        {
            body = Body;
            if (ContentType.Contains(";"))
            {
                //Sanitize content-types for uniform matching later on
                contentType = ContentType.Split(";")[0];
            }
            else
            {
                contentType = ContentType;
            }
            return this;
        }

        public ValidationBuilder WithHeaders(HttpHeaders Headers)
        {
            headers = Headers;
            return this;
        }

        public Validator Build()
        {
            val.pathVal = new PathValidator(path);
            val.methodVal = new MethodValidator(method);
            val.statusCodeVal = new StatusCodeValidator(statusCode, body);
            val.bodyVal = new BodyValidator(body, contentType);
            val.headerVal = new HeaderValidator(headers);
            return val;
        }
    }
}
