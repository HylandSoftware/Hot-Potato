using NJsonSchema.Validation;

namespace HotPotato.Validators
{
    public class HotPotatoValidationError : IHotPotatoValidationError
    {
        public string Message { get; set; }
        public string Kind { get; set; }
        public string Property { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }

        public HotPotatoValidationError(ValidationError valErr)
        {
            this.Message = valErr.ToString();
            this.Kind = valErr.Kind.ToString();
            this.Property = valErr.Property;
            this.LineNumber = valErr.LineNumber;
            this.LinePosition = valErr.LinePosition;
        }

    }
}
