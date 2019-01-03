using System;
using System.Collections.Generic;
using System.Text;
using NSwag;
using HotPotato.Models;
using Microsoft.AspNetCore.Http;

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
