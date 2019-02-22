using HotPotato.Core.Models;
using HotPotato.OpenApi.Validators;
using System;

namespace HotPotato.OpenApi.Locators
{
    public interface ILocator
    {
        Tuple<IBodyValidator, IHeaderValidator> GetValidator(HttpPair pair);
    }
}
