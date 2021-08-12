using HotPotato.Core.Http;
using HotPotato.OpenApi.Validators;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace HotPotato.OpenApi.Models
{
	public class FailResultWithCustomHeadersTest
	{
		private readonly string[] expectedKeys = { "Custom", "State", "Path", "Method", "StatusCode", "Reasons", "ValidationErrors" };

		private const string path = "/path";
		private const string method = "trace";
		private const int statusCode = 200;
		private readonly List<Reason> reason = new List<Reason>() { Reason.InvalidBody };
		private readonly List<ValidationError> validationErrors = new List<ValidationError>(){
			new ValidationError("message", ValidationErrorKind.ArrayExpected, "property", 0, 0),
			new ValidationError("anothermessage", ValidationErrorKind.BooleanExpected, "anotherproperty", 0, 0)
		};

		private const string AValidCustomHeaderKey = "X-HP-Custom-Header-Key";
		private const string AValidCustomHeaderValue = "Custom-Header-Value";

		[Fact]
		public void FailResultWithCustomHeaders_SerializesWithCustomHeaderFirst()
		{
			var customHeaders = new HttpHeaders();
			customHeaders.Add(AValidCustomHeaderKey, AValidCustomHeaderValue);

			FailResultWithCustomHeaders subject = new FailResultWithCustomHeaders(path, method, statusCode, reason, customHeaders, validationErrors);

			JObject result = JObject.FromObject(subject);
			JToken property = result.First;

			foreach (string expectedKey in expectedKeys)
			{
				Assert.Equal(expectedKey, property.Path);
				property = property.Next;
			}
		}
	}
}
