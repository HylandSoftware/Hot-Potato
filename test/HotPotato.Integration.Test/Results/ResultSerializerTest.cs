using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using System;
using System.IO;
using System.Net;
using Xunit;

namespace HotPotato.Integration.Test.Results
{
    public class ResultSerializerTest
    {
        private const string Path = @"M:\Git\HotPotato .NET\test\HotPotato.Integration.Test\JsonTestData.json";

        [Fact]
        public void CanISerializeListFromResultCollector()
        {
            ValidationError er = new ValidationError("message", ValidationErrorKind.Unknown, "property", 10, 15);
            ValidationError er2 = new ValidationError("Not Found", ValidationErrorKind.NullExpected, "property", 5, 28);
            ValidationError[] arr = new ValidationError[] { er, er2 };

            ResultCollector collector = new ResultCollector();

            collector.Pass(GetNewPair("/endpoint", HttpStatusCode.OK));
            collector.Pass(GetNewPair("/endpoint2", HttpStatusCode.Accepted));
            collector.Fail(GetNewPair("/endpoint3", HttpStatusCode.BadRequest), Reason.Unknown, er);
            collector.Fail(GetNewPair("/endpoint4", HttpStatusCode.NotFound), Reason.MissingPath, arr);

            JsonResultSerializer serializer = new JsonResultSerializer();

            var actual = serializer.SerializeResult(collector.resultList);

            using (StreamReader reader = new StreamReader(Path))
            {
                string expected = reader.ReadToEnd();

                Assert.Equal(expected, actual);
            }
        }

        private HttpPair GetNewPair(string endpoint, HttpStatusCode code)
        {
            using (IHttpRequest req = new HttpRequest(new Uri("http://localhost" + endpoint)))
            {
                IHttpResponse res = new HttpResponse(code, new HttpHeaders());

                using(HttpPair pair = new HttpPair(req, res)){

                    return pair;
                }
            }

        }
    }
}
