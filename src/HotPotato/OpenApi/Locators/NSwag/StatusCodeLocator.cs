using HotPotato.Models;
using Microsoft.AspNetCore.Http;
using NSwag;
using System;

namespace HotPotato.OpenApi.Locators.NSwag
{
    class StatusCodeLocator
    {
        public SwaggerResponse Locate(HttpPair pair, SwaggerOperation operation)
        {
            string statusCode = Enum.GetName(typeof(StatusCodes), pair.Response.StatusCode);
            return operation.Responses[statusCode];
        }
    }
}
