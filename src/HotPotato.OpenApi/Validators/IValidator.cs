using HotPotato.Core.Models;

namespace HotPotato.OpenApi.Validators
{
    public interface IValidator
    {
        void Validate(HttpPair pair);
    }
}
