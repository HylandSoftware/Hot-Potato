using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Exceptions;
using HotPotato.Models;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Locators.Default
{
    internal class Locator : ILocator
    {
        private PathLocator PathLocator { get; }
        private MethodLocator MethodLocator { get;  }
        private StatusCodeLocator StatusCodeLocator { get; }

        public Locator(PathLocator pathLocator, MethodLocator methodLocator, StatusCodeLocator statusCodeLocator)
        {
            _ = pathLocator ?? throw new ArgumentNullException(nameof(pathLocator));
            _ = methodLocator ?? throw new ArgumentNullException(nameof(methodLocator));
            _ = statusCodeLocator ?? throw new ArgumentNullException(nameof(statusCodeLocator));

            PathLocator = pathLocator;
            MethodLocator = methodLocator;
            StatusCodeLocator = statusCodeLocator;
        }

        public JsonSchema4 GetSchema(HttpPair pair, SwaggerDocument document)
        {
            SwaggerPathItem path = PathLocator.Locate(pair, document);
            _ = path ?? throw new LocatorException();
            SwaggerOperation operation = MethodLocator.Locate(pair, path);
            _ = operation ?? throw new LocatorException();
            SwaggerResponse response = StatusCodeLocator.Locate(pair, operation);
            _ = response ?? throw new LocatorException();
            return response?.ActualResponseSchema;
        }
    }
}
