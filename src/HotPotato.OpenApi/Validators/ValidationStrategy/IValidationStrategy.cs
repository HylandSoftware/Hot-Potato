using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    public interface IValidationStrategy
    {
        void Validate();
    }
}
