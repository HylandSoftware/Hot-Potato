using HotPotato.Core;
using NSwag;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    internal class MethodValidator
    {
        public string Method { get; }
        public SwaggerOperation Result { get; private set; }
        
        public MethodValidator(HttpMethod method)
        {
            if (method == null)
            {
                Method = string.Empty;
            }
            else
            {
                Method = toOperationMethod(method.ToString());
            }
        }

        public bool Validate(SwaggerPathItem swagPath)
        {
            if (swagPath.ContainsKey(Method))
            {
                Result = swagPath[Method];
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
