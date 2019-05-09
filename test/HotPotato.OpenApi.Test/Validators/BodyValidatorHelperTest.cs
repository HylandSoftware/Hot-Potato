using HotPotato.OpenApi.Models;
using NJsonSchema;
using NSwag;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class BodyValidatorHelperTest
    {
        [Fact]
        public void ConvertBodyString_XmlToJsonString()
        {
            string inputXML = @"<LGNotification><MediaType>video</MediaType><StatusFlag>new</StatusFlag><URL>http://domain.com/program/app?clienttype=htmlamp;id=49977</URL></LGNotification>";
            BodyValidator bodVal = new BodyValidator(inputXML, "application/xml");
            bodVal.ConvertBodyString();
            Assert.Equal("{\"LGNotification\":{\"MediaType\":\"video\",\"StatusFlag\":\"new\",\"URL\":\"http://domain.com/program/app?clienttype=htmlamp;id=49977\"}}", bodVal.BodyString);
        }

        [Fact]
        public void ConvertBodyString_TextToJsonString()
        {
            string inputText = "Content";
            BodyValidator bodVal = new BodyValidator(inputText, "text/plain");
            bodVal.ConvertBodyString();
            Assert.Equal("\"Content\"", bodVal.BodyString);
        }
    }
}
