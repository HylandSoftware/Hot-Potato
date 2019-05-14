using Xunit;

namespace HotPotato.Core.Http
{
    public class HttpContentTypeTest
    {
        private const string AValidTypeWithoutSemicolon = "application/json";
        private const string AValidTypeWithSemicolon = "application/json;charset=utf-8";
        private const string DefaultJsonEncoding = "utf-8";

        [Fact]
        public void Constructor_SetsProperties()
        {
            HttpContentType subject = new HttpContentType(AValidTypeWithoutSemicolon, DefaultJsonEncoding);
            Assert.Equal(AValidTypeWithoutSemicolon, subject.Type);
            Assert.Equal(DefaultJsonEncoding, subject.CharSet);
        }

        [Fact]
        public void Constructor_SplitsSemicolon()
        {
            HttpContentType subject = new HttpContentType(AValidTypeWithSemicolon);
            Assert.Equal(AValidTypeWithoutSemicolon, subject.Type);
        }

        [Fact]
        public void ConstructorSets_DefaultEncoding()
        {
            HttpContentType subject = new HttpContentType(AValidTypeWithoutSemicolon);
            Assert.Equal(DefaultJsonEncoding, subject.CharSet);
        }
    }
}
