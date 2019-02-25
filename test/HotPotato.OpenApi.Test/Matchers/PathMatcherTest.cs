using System.Collections.Generic;
using Xunit;

namespace HotPotato.OpenApi.Matchers
{
    public class PathMatcherTest
    {
        [Fact]
        public void Match_WithNormalPaths()
        {
            string AValidPath = "/foo/bar";
            List<string> AValidPaths = new List<string>
            {
                "/a/b/c",
                "/a/b",
                "/foo/bar",
                "/d/c"
            };

            string result = PathMatcher.Match(AValidPath, AValidPaths);

            Assert.Equal("/foo/bar", result);
        }

        [Fact]
        public void Match_WithParameters()
        {
            string AValidPath = "/foo/1/bar";
            List<string> AValidPaths = new List<string>
            {
                "/a/b/{c}",
                "/foo/{docID}/bar",
                "/b/d/c"
            };

            string result = PathMatcher.Match(AValidPath, AValidPaths);
            Assert.Equal("/foo/{docID}/bar", result);
        }

        [Fact]
        public void Match_ParamAtEnd()
        {
            string AValidPath = "/foo/1";
            List<string> AVAlidPaths = new List<string>
            {
                "/a/b/{c}",
                "/foo/{bar}",
                "/d/{e}/f"
            };

            string result = PathMatcher.Match(AValidPath, AVAlidPaths);

            Assert.Equal("/foo/{bar}", result);
        }

        [Fact]
        public void Match_NotInList_ReturnsEmpty()
        {
            string AValidPath = "/foo/bar";
            List<string> AValidPaths = new List<string>
            {
                "/a/b/{c}",
                "/foo",
                "/bar/{c}"
            };

            string result = PathMatcher.Match(AValidPath, AValidPaths);

            Assert.Equal(string.Empty, result);
        }
    }
}
