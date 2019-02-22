using HotPotato.OpenApi.Results;

namespace HotPotato.OpenApi.Validators
{
    public interface IBodyValidator
    {
        Result Validate(string content);
    }
}
