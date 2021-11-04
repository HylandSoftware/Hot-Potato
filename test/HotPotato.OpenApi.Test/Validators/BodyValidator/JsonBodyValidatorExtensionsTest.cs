using System;
using HotPotato.OpenApi.Models;
using Moq;
using NJsonSchema;
using System.Collections.Generic;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
	public class JsonBodyValidatorExtensionsTest
	{
		private const string AValidBody = "{'foo': 1}";
		private const string AValidSchema = @"{'properties':{'foo':{'type':'integer'}}}";
		private const string ABodyWithAnUnexpectedProperty = "{'bar': 2}";

		[Fact]
		public void ValidateUndefinedProperties_ReturnsEmptyList_WithValidSchema()
		{
			JsonSchema subject = JsonSchema.FromJsonAsync(AValidSchema).Result;

			List<ValidationError> results = subject.ValidateUndefinedProperties(AValidBody);

			Assert.Empty(results);
		}

		[Fact]
		public void ValidateUndefinedProperties_ReturnsEmptyList_WithNullActualSchema()
		{
			JsonSchema nullSchema = null;
			Mock<JsonSchema> subject = new Mock<JsonSchema>();
			subject.SetupGet(x => x.ActualSchema).Returns(nullSchema);

			List<ValidationError> results = subject.Object.ValidateUndefinedProperties(AValidBody);

			Assert.Empty(results);
		}

		[Fact]
		public void JsonBodyValidator_ReturnsAListWithCorrectValidationErrorKind_WithInvalidSchema()
		{
			JsonSchema subject = JsonSchema.FromJsonAsync(AValidSchema).Result;

			List<ValidationError> results = subject.ValidateUndefinedProperties(ABodyWithAnUnexpectedProperty);

			Assert.Equal(ValidationErrorKind.PropertyNotInSpec, results[0].Kind);
		}

		[Fact]
		public void JsonBodyValidator_ReturnsAListWithCorrectValidationErrorKind_WithBlankSchema()
		{
			JsonSchema subject = new JsonSchema();

			List<ValidationError> results = subject.ValidateUndefinedProperties(ABodyWithAnUnexpectedProperty);

			Assert.Equal(ValidationErrorKind.PropertyNotInSpec, results[0].Kind);
		}
	}
}
