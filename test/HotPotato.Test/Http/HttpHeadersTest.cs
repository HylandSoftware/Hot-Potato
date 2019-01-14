using System.Collections.Generic;
using Xunit;

namespace HotPotato.Http
{
    public class HttpHeadersTest
    {
        private const string AValidKey = "AValidKey";
        private const string AValidValue = "AValidValue";
        private const string AnotherValidValue = "AnotherValidValue";

        [Fact]
        public void Add_KeyDoesntExist_CreatesKey()
        {
            HttpHeaders subject = new HttpHeaders();
            Assert.False(subject.ContainsKey(AValidKey));
            subject.Add(AValidKey, AValidValue);
            Assert.True(subject.ContainsKey(AValidKey));
        }

        [Fact]
        public void Add_KeyDoesExist_AddsToList()
        {
            HttpHeaders subject = new HttpHeaders();
            subject.Add(AValidKey, AValidValue);
            Assert.True(subject.ContainsKey(AValidKey));
            subject.Add(AValidKey, AnotherValidValue);
            Assert.True(subject.ContainsKey(AValidKey));
            List<string> result = subject[AValidKey];
            Assert.Equal(2, result.Count);
            Assert.Contains(AnotherValidValue, result);
        }

        [Fact]
        public void Add_List_AppliesListToKey()
        {
            HttpHeaders subject = new HttpHeaders();
            subject.Add(AValidKey, new List<string>() { AValidValue, AnotherValidValue });
            List<string> result = subject[AValidKey];
            Assert.NotNull(result);
            Assert.Contains(AValidValue, result);
            Assert.Contains(AnotherValidValue, result);
        }
    }
}
