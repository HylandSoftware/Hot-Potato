using HotPotato.OpenApi.Models;

namespace HotPotato.OpenApi.Validators
{
    public interface IValidationStrategy
    {
        void Validate();
        void AddValidationResult(IValidationResult bodyResult, IValidationResult headerResult);
    }
}
