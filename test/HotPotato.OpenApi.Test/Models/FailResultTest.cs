using HotPotato.OpenApi.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xunit;

namespace HotPotato.OpenApi.Models
{
	public class FailResultTest
	{
		private readonly string[] expectedKeys = { "State", "Path", "Method", "StatusCode", "Reasons", "ValidationErrors" };
		private readonly string singleExpectedReason = "[\"InvalidBody\"]";
		private readonly string multipleExpectedReasons = "[\"InvalidBody\",\"InvalidHeaders\"]";
		private readonly string expectedErrors = "\"ValidationErrors\":[{\"Message\":\"message\",\"Kind\":\"ArrayExpected\",\"Property\":\"property\",\"LineNumber\":0,\"LinePosition\":0},{\"Message\":\"anothermessage\",\"Kind\":\"BooleanExpected\",\"Property\":\"anotherproperty\",\"LineNumber\":0,\"LinePosition\":0}]";

		private const string path = "/path";
		private const string method = "trace";
		private const int statusCode = 200;
		private readonly List<Reason> reason = new List<Reason>() { Reason.InvalidBody };
		private readonly List<Reason> reasons = new List<Reason>() { Reason.InvalidBody, Reason.InvalidHeaders };
		private readonly List<ValidationError> validationErrors = new List<ValidationError>(){
			new ValidationError("message", ValidationErrorKind.ArrayExpected, "property", 0, 0),
			new ValidationError("anothermessage", ValidationErrorKind.BooleanExpected, "anotherproperty", 0, 0)
		};

		[Fact]
		public void FailResult_SerializesWithExpectedKeysInOrder()
		{
			FailResult subject = new FailResult(path, method, statusCode, reason, validationErrors);

			JObject result = JObject.FromObject(subject);
			JToken property = result.First;          

			foreach (string expectedKey in expectedKeys)
			{
				Assert.Equal(expectedKey, property.Path);
				property = property.Next;
			}
		}

		[Fact]
		public void FailResult_ReturnsReasonsCollectionFromSingle()
		{
			FailResult subject = new FailResult(path, method, statusCode, reason, validationErrors);

			string result = JsonConvert.SerializeObject(subject);

			Assert.Contains(singleExpectedReason, result);
		}

		[Fact]
		public void FailResult_ReturnsReasonsCollectionFromMultiple()
		{
			FailResult subject = new FailResult(path, method, statusCode, reasons, validationErrors);

			string result = JsonConvert.SerializeObject(subject);

			Assert.Contains(multipleExpectedReasons, result);
		}

		[Fact]
		public void FailResult_ReturnsValidationErrorCollection()
		{
			FailResult subject = new FailResult(path, method, statusCode, reasons, validationErrors);

			string result = JsonConvert.SerializeObject(subject);

			Assert.Contains(expectedErrors, result);
		}
	}
}
