using HotPotato.Core.Models;
using HotPotato.Validators;
using System;

namespace HotPotato.OpenApi.Locators
{
    public interface ILocator
    {
        Tuple<IBodyValidator, IHeaderValidator> GetValidator(HttpPair pair);
    }
}
