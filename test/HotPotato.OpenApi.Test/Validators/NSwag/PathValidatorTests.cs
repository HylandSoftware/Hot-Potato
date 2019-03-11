using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Moq;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators.NSwag
{
    public class PathValidatorTests
    {
        [Fact]
        public void PathValidator_CallsFail()
        {
            IHttpResponse mockResp = Mock.Of<IHttpResponse>();

            using (IHttpRequest mockReq = Mock.Of<IHttpRequest>())
            using (HttpPair testPair = new HttpPair(mockReq, mockResp))
            {
                mockReq.Dispose();
                Mock<IResultCollector> mockColl = new Mock<IResultCollector>();
                PathValidator subject = new PathValidator();
                subject.Validate(null, testPair, mockColl.Object);
                mockColl.Verify(m => m.Fail(testPair, Reason.MissingPath));
            }
        }

        [Fact]
        public void PathValidator_CallsPass()
        {
            SwaggerPathItem mockSwagPath = Mock.Of<SwaggerPathItem>();
            IHttpResponse mockResp = Mock.Of<IHttpResponse>();

            using(IHttpRequest mockReq = Mock.Of<IHttpRequest>())
            using (HttpPair testPair = new HttpPair(mockReq, mockResp))
            {
                Mock<IResultCollector> mockColl = new Mock<IResultCollector>();
                PathValidator subject = new PathValidator();
                subject.Validate(mockSwagPath, testPair, mockColl.Object);
                mockColl.Verify(m => m.Pass(testPair));
            }
        }
    }
}