using System;
using System.Net.Http;
using Xunit;

namespace HotPotato.Core.Http.Default
{
    public class HttpRequestTest
    {
        private const string AValidUriWithPipes = "http://foo/16|32|48";
        private const string ExpectedPath = "/16|32|48";
        private const string AValidRelativePath = "content-types";

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

        [Fact]
        public void HttpRequest_Ctor_CanUseRelativeUris()
        {
            Uri expectedUri = new Uri(AValidRelativePath, UriKind.Relative);

            HttpRequest subject = new HttpRequest(expectedUri);

            Assert.Equal(expectedUri, subject.Uri);
            Assert.Equal(AValidRelativePath, subject.DecodedPath);
        }

        [Fact]
        public void HttpRequest_CtorWithMethod_CanUseRelativeUris()
		{
            Uri expectedUri = new Uri(AValidRelativePath, UriKind.Relative);

            HttpRequest subject = new HttpRequest(HttpMethod.Trace, expectedUri);

            Assert.Equal(expectedUri, subject.Uri);
            Assert.Equal(AValidRelativePath, subject.DecodedPath);
        }
    }
}
