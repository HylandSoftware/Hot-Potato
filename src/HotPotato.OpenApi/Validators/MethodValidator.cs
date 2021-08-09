using HotPotato.Core;
using NSwag;
using System.Net.Http;

namespace HotPotato.OpenApi.Validators
{
    internal class MethodValidator
    {
        public string Method { get; }
        public OpenApiOperation Result { get; private set; }
        
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

        public bool Validate(OpenApiPathItem swagPath)
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

        private static string toOperationMethod(string method)
        {
            switch (method)
            {
                case HttpVerbs.DELETE:
                    return OpenApiOperationMethod.Delete;
                case HttpVerbs.GET:
                    return OpenApiOperationMethod.Get;
                case HttpVerbs.OPTIONS:
                    return OpenApiOperationMethod.Options;
                case HttpVerbs.PATCH:
                    return OpenApiOperationMethod.Patch;
                case HttpVerbs.POST:
                    return OpenApiOperationMethod.Post;
                case HttpVerbs.PUT:
                    return OpenApiOperationMethod.Put;
                case HttpVerbs.TRACE:
                    return OpenApiOperationMethod.Trace;
                default:
                    return OpenApiOperationMethod.Undefined;
            }
        }
    }
}
