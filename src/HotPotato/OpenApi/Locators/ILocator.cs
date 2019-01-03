using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Models;
using HotPotato.Validators;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Locators
{
    public interface ILocator
    {
        Tuple<IBodyValidator, IHeaderValidator> GetValidator(HttpPair pair);
    }
}
