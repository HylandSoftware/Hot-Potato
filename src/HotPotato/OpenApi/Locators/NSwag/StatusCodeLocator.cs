using HotPotato.Models;
//using Microsoft.AspNetCore.Http;
using System.Net;
using NSwag;
using System;

namespace HotPotato.OpenApi.Locators.NSwag
{
    class StatusCodeLocator
    {
        public SwaggerResponse Locate(HttpPair pair, SwaggerOperation operation)
        {
            string statusCode = Convert.ToInt32(pair.Response.StatusCode).ToString();
            return operation.Responses[statusCode];
        }
    }
}
