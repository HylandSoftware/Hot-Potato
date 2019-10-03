using HotPotato.Core.Http;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using System.Net;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationBuilder
    {
        private string Path { get; set; }
        private HttpMethod Method { get; set; }
        private HttpStatusCode StatusCode { get; set; }
        private string Body { get; set; }
        private HttpContentType ContentType { get; set; }
        private HttpHeaders Headers { get; set; }
        private HttpHeaders CustomHeaders { get; set; }

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

        public ValidationBuilder WithBody(string body, HttpContentType contentType)
        {
            Body = body;
            ContentType = contentType;
            return this;
        }

        public ValidationBuilder WithHeaders(HttpHeaders headers)
        {
            Headers = headers;
            return this;
        }

        public ValidationBuilder WithCustomHeaders(HttpHeaders customHeaders)
        {
            CustomHeaders = customHeaders;
            return this;
        }

        public IValidationStrategy Build()
        {
            ValidationStrategy val = new ValidationStrategy(ResultCollector, SpecificationProvider, ContentType);
            val.PathValidator = new PathValidator(Path);
            val.MethodValidator = new MethodValidator(Method);
            val.StatusCodeValidator = new StatusCodeValidator(StatusCode, Body);
            val.ContentValidator = new ContentValidator(Body, ContentType);
            val.BodyValidator = BodyValidatorFactory.Create(Body, ContentType);
            val.HeaderValidator = new HeaderValidator(Headers);
            val.CustomHeaders = CustomHeaders;
            return val;
        }
    }
}
