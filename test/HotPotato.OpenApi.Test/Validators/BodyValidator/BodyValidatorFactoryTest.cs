using HotPotato.Core.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class BodyValidatorFactoryTest
    {
        private const string AValidJsonBody = "{'foo': '1'}";
        private const string AValidTextBody = "Content";
        private const string AValidXmlBody = @"<LGNotification><MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";

        [Fact]
        public void BodyValidatorFactory_Create_ReturnsJson()
        {
            BodyValidator result = BodyValidatorFactory.Create(AValidJsonBody, new HttpContentType("application/json"));

            Assert.IsType<JsonBodyValidator>(result);
        }

        [Fact]
        public void BodyValidator_Create_ReturnsText()
        {
            BodyValidator result = BodyValidatorFactory.Create(AValidTextBody, new HttpContentType("text/plain"));

            Assert.IsType<TextBodyValidator>(result);
        }

        [Fact]
        public void BodyValidator_Create_ReturnsXml()
        {
            BodyValidator result = BodyValidatorFactory.Create(AValidXmlBody, new HttpContentType("application/xml"));

            Assert.IsType<XmlBodyValidator>(result);
        }
    }
}
