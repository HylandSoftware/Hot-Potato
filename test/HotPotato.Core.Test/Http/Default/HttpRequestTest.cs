using System;
using System.Net.Http;
using Xunit;

namespace HotPotato.Core.Http.Default
{
    public class HttpRequestTest
    {
        private const string AValidUriWithPipes = "http://foo/16|32|48";
        private const string ExpectedPath = "/16|32|48";
        [Fact]
        public void HttpRequest_DecodedPath_DecodesSpecialCharacters()
        {
            HttpRequest subject = new HttpRequest(new Uri(AValidUriWithPipes));
            string result = subject.DecodedPath;
            Assert.Equal(ExpectedPath, result);
        }

        [Fact]
        public void HttpRequest_DecodedPath_DecodesSpecialCharactersWithMethodCtor()
        {
            HttpRequest subject = new HttpRequest(HttpMethod.Trace, new Uri(AValidUriWithPipes));
            string result = subject.DecodedPath;
            Assert.Equal(ExpectedPath, result);
        }
    }
}
