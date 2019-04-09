using HotPotato.Core;
using NSwag;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    internal class MethodValidator
    {
        public string method { get; }
        public SwaggerOperation Result { get; set; }
        
        public MethodValidator(HttpMethod Method)
        {
            if (Method == null)
            {
                method = "";
            }
            else
            {
                method = toOperationMethod(Method.ToString());
            }
        }

        public bool Validate(SwaggerPathItem swagPath)
        {
            if (swagPath.ContainsKey(method))
            {
                Result = swagPath[method];
                return true;
            }
            else
            {
                return false;
            }
        }

        private string toOperationMethod(string method)
        {
            switch (method)
            {
                case HttpVerbs.DELETE:
                    return SwaggerOperationMethod.Delete;
                case HttpVerbs.GET:
                    return SwaggerOperationMethod.Get;
                case HttpVerbs.OPTIONS:
                    return SwaggerOperationMethod.Options;
                case HttpVerbs.PATCH:
                    return SwaggerOperationMethod.Patch;
                case HttpVerbs.POST:
                    return SwaggerOperationMethod.Post;
                case HttpVerbs.PUT:
                    return SwaggerOperationMethod.Put;
                case HttpVerbs.TRACE:
                    return SwaggerOperationMethod.Trace;
                default:
                    return SwaggerOperationMethod.Undefined;
            }
        }
    }
}
