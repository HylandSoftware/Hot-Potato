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
            Mock<IHttpRequest> mockReq = new Mock<IHttpRequest>();
            Mock<IHttpResponse> mockResp = new Mock<IHttpResponse>();
            HttpPair testPair = new HttpPair(mockReq.Object, mockResp.Object);
            Mock <IResultCollector> mockColl = new Mock<IResultCollector>();

            PathValidator subject = new PathValidator();
            subject.Validate(null, testPair, mockColl.Object);
            mockColl.Verify(m => m.Fail(testPair, Reason.MissingPath));
        }

        [Fact]
        public void PathValidator_CallsPass()
        {
            Mock<SwaggerPathItem> mockSwagPath = new Mock<SwaggerPathItem>();
            Mock<IHttpRequest> mockReq = new Mock<IHttpRequest>();
            Mock<IHttpResponse> mockResp = new Mock<IHttpResponse>();
            HttpPair testPair = new HttpPair(mockReq.Object, mockResp.Object);
            Mock<IResultCollector> mockColl = new Mock<IResultCollector>();

            PathValidator subject = new PathValidator();
            subject.Validate(mockSwagPath.Object, testPair, mockColl.Object);
            mockColl.Verify(m => m.Pass(testPair));
        }
    }
}
