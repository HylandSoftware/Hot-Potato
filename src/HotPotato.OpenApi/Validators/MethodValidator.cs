using HotPotato.Core;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using NSwag;
using System;

namespace HotPotato.OpenApi.Validators
{
    internal class MethodValidator : IValidator
    {
        private readonly IValidationProvider valPro;
        private readonly IResultCollector collector;
        public MethodValidator(IValidationProvider valPro, IResultCollector collector)
        {
            this.valPro = valPro;
            this.collector = collector;
        }
        public void Validate(HttpPair pair)
        {
            _ = pair ?? throw new ArgumentNullException(nameof(pair));

            string method = toOperationMethod(pair.Request.Method.ToString());
            SwaggerPathItem swagPath = valPro.specPath;
            if (swagPath.ContainsKey(method))
            {
                valPro.specMeth = swagPath[method];
            }
            else
            {
                collector.Fail(pair, Reason.MissingMethod);
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
