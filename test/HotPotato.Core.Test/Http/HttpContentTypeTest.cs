using Xunit;

namespace HotPotato.Core.Http
{
    public class HttpContentTypeTest
    {
        private const string AValidType = "application/json";
        private const string DefaultJsonEncoding = "utf-8";

        [Fact]
        public void Constructor_SetsProperties()
        {
            HttpContentType subject = new HttpContentType(AValidType, DefaultJsonEncoding);
            Assert.Equal(AValidType, subject.Type);
            Assert.Equal(DefaultJsonEncoding, subject.CharSet);
        }

        [Fact]
        public void ConstructorSets_DefaultEncoding()
        {
            HttpContentType subject = new HttpContentType(AValidType);
            Assert.Equal(DefaultJsonEncoding, subject.CharSet);
        }
    }
}
