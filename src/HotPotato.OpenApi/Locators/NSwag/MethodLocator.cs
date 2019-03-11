using HotPotato.Core;
using HotPotato.Core.Models;
using NSwag;

namespace HotPotato.OpenApi.Locators.NSwag
{
    internal class MethodLocator
    {
        public SwaggerOperation Locate(HttpPair pair, SwaggerPathItem path)
        {
            string method = toOperationMethod(pair.Request.Method.ToString());
            return path[method];
        }

        //NSwag v12.0.15 - SwaggerOperation method is now a class that returns strings
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
