
using System;
using Xunit;

namespace HotPotato.Validators
{
    public class ValidationErrorTest
    {
        [Fact]
        public void NJsonSchemaErrorKinds_MatchHotPotato()
        {
            string[] subject = Enum.GetNames(typeof(NJsonSchema.Validation.ValidationErrorKind));
            string[] hotPotKinds = Enum.GetNames(typeof(ValidationErrorKind));
            foreach (string kind in subject)
            {
                Assert.Contains(kind, hotPotKinds);
            }
        }

        [Fact]
        public void ToErrorKind_MatchesHotPotatoKind()
        {
            ValidationErrorKind subject = NJsonSchema.Validation.ValidationErrorKind.IntegerTooBig.ToString().ToErrorKind();
            Assert.Equal(ValidationErrorKind.IntegerTooBig, subject);
        }
    }
}
