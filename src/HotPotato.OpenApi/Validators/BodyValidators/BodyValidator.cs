using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using NJsonSchema;

namespace HotPotato.OpenApi.Validators
{
	internal abstract class BodyValidator
	{
		public string BodyString { get; protected set; }
		public HttpContentType ContentType { get; protected set; }

		public abstract IValidationResult Validate(JsonSchema schema);
	}
}
