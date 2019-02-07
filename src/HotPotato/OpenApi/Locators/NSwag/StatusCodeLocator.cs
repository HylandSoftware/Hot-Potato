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
<<<<<<< HEAD
            //string statusCode = Enum.GetName(typeof(StatusCodes), pair.Response.StatusCode);
            string statusCode = Enum.GetName(typeof(HttpStatusCode), pair.Response.StatusCode);
=======
            string statusCode = Convert.ToInt32(pair.Response.StatusCode).ToString();
>>>>>>> bugfix/AUTOTEST-184-correct-identifier-for-path-locator
            return operation.Responses[statusCode];
        }
    }
}
