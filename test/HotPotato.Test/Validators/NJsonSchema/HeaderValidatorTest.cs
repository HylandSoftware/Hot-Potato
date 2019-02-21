﻿using HotPotato.Http;
using HotPotato.Results;
using static HotPotato.Results.ResultsMethods;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;
using Xunit;

namespace HotPotato.Validators
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

            Assert.Equal(ValidationErrorKind.IntegerExpected, GetInvalidReasons(enumerator.Current)[0].Kind.ToString().ToErrorKind());
            Assert.IsAssignableFrom<HeaderInvalidResult>(enumerator.Current);
        }
    }
}
