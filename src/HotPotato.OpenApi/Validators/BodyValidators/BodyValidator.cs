using HotPotato.Core.Http;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal abstract class BodyValidator
    {
        public string BodyString { get; protected set; }
        public HttpContentType ContentType { get; protected set; }

        public abstract IValidationResult Validate(SwaggerResponse swagResp);
    }
}
