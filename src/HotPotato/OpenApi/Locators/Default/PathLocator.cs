using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Models;
using NSwag;

namespace HotPotato.OpenApi.Locators.Default
{
    internal class PathLocator
    {
        public SwaggerPathItem Locate(HttpPair pair, SwaggerDocument swaggerDocument)
        {
            string path = pair.Request.Uri.AbsolutePath;
            //swaggerDocument.Paths.Keys;

            throw new NotImplementedException();
        }
    }
}
