using HotPotato.Results;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HotPotato.Validators
{
    public class BodyValidatorTest
    {
        private const string AValidBody = "{'foo': '1'}";
        private const string AnInvalidBody = "{'foo': 'abc'}";
        private const string AValidSchema = @"{'type': 'integer'}";

        [Fact]
        public void Validate_ValidBody()
        {
            JsonSchema4 schema = JsonSchema4.CreateAnySchema();
            BodyValidator subject = new BodyValidator();

            Result result = subject.Validate(AValidBody, schema);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<BodyValidResult>(result);
        }

        [Fact]
        public void Validate_InvalidBody()
        {
            JsonSchema4 schema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            BodyValidator subject = new BodyValidator();

            Result result = subject.Validate(AnInvalidBody, schema);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<BodyInvalidResult>(result);
        }
    }
}
