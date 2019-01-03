using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using HotPotato.Http;
using NSwag;
using NJsonSchema;
using HotPotato.Results;

namespace HotPotato.Validators.NJsonSchema
{
    public class HeaderValidatorTest
    {
        private const string AValidHeaderKey = "X-Header-Key";
        private const string AValidHeaderValue = "value";
        private const string AValidSchema = @"{'type': 'integer'}";
        private const string AnInvalidValue = "invalidValue";

        [Fact]
        public void Validate_KeyNotFound()
        {
            HttpHeaders headers = new HttpHeaders();
            SwaggerHeaders schema = new SwaggerHeaders();
            schema.Add(AValidHeaderKey, new JsonSchema4());
            HeaderValidator subject = new HeaderValidator(schema);

            ICollection<Result> result = subject.Validate(headers);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            IEnumerator<Result> enumerator = result.GetEnumerator();
            enumerator.MoveNext();
            Assert.IsAssignableFrom<HeaderNotFoundResult>(enumerator.Current);
        }

        [Fact]
        public void Validate_ValidSchema()
        {
            HttpHeaders headers = new HttpHeaders();
            headers.Add(AValidHeaderKey, AValidHeaderValue);
            JsonSchema4 jsonSchema = JsonSchema4.CreateAnySchema();
            SwaggerHeaders schema = new SwaggerHeaders();
            schema.Add(AValidHeaderKey, jsonSchema);
            HeaderValidator subject = new HeaderValidator(schema);

            ICollection<Result> result = subject.Validate(headers);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            IEnumerator<Result> enumerator = result.GetEnumerator();
            enumerator.MoveNext();
            Assert.IsAssignableFrom<HeaderValidResult>(enumerator.Current);
        }

        [Fact]
        public void Validate_InvalidSchema()
        {
            HttpHeaders headers = new HttpHeaders();
            headers.Add(AValidHeaderKey, AnInvalidValue);
            JsonSchema4 jsonSchema = JsonSchema4.FromJsonAsync(AValidSchema).Result;
            SwaggerHeaders schema = new SwaggerHeaders();
            schema.Add(AValidHeaderKey, jsonSchema);
            HeaderValidator subject = new HeaderValidator(schema);

            ICollection<Result> result = subject.Validate(headers);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            IEnumerator<Result> enumerator = result.GetEnumerator();
            enumerator.MoveNext();
            Assert.IsAssignableFrom<HeaderInvalidResult>(enumerator.Current);
        }
    }
}
