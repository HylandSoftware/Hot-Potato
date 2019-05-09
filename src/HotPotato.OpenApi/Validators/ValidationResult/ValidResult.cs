
namespace HotPotato.OpenApi.Validators
{
    public class ValidResult : IValidationResult
    {
        public bool Valid { get; }
        public ValidResult()
        {
            Valid = true;
        }
    }
}
