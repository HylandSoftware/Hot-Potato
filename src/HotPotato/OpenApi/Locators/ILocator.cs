using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Models;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Locators
{
    public interface ILocator
    {
        JsonSchema4 GetSchema(HttpPair pair, SwaggerDocument document);
    }
}
