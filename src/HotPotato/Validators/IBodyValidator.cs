using HotPotato.Results;

namespace HotPotato.Validators
{
    public interface IBodyValidator
    {
        Result Validate(string content);
    }
}
