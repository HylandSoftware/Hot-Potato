
namespace HotPotato.OpenApi.Validators
{
	public class ValidationError
	{
		public string Message { get; set; }
		[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
		public ValidationErrorKind Kind { get; set; }
		public string Property { get; set; }
		public int LineNumber { get; set; }
		public int LinePosition { get; set; }

		public ValidationError(string message, ValidationErrorKind kind, string property, int lineNumber, int linePosition)
		{
			this.Message = message;
			this.Kind = kind;
			this.Property = property;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
