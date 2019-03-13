using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
using Xunit;

namespace HotPotato.Results
{
    public class JsonResultSerializerTest
    {
        private const string Path = "/endpoint";
        private const string Get = "GET";
        private const int OkStatusCode = 200;
        private const int NotFoundStatusCode = 404;

        private const string PassJson = "[{\"Path\":\"/endpoint\",\"Method\":\"GET\",\"StatusCode\":200,\"State\":\"Pass\",\"Reason\":\"Unknown\",\"ValidationErrors\":null}]";
        private const string FailJson = "[{\"Path\":\"/otherPath\",\"Method\":\"Post\",\"StatusCode\":404,\"State\":\"Fail\",\"Reason\":\"MissingMethod\",\"ValidationErrors\":[{\"Message\":\"Error\",\"Kind\":\"Unknown\",\"Property\":\"Property\",\"LineNumber\":5,\"LinePosition\":10}]}]";

        [Fact]
        public void CanISerializePassResultToJsonString()
        {
            OpenApi.Models.Result result = new OpenApi.Models.Result(Path, Get, OkStatusCode, State.Pass);
            

            List<OpenApi.Models.Result> resultList = new List<OpenApi.Models.Result>()
            {
                result
            };

            var subject = new JsonResultSerializer();

            var actual = subject.SerializeResult(resultList);

            Assert.Equal(PassJson, actual);
        }

        [Fact]
        public void CanISerializeFailResultToJsonString()
        {
            var err = new ValidationError("Error", ValidationErrorKind.Unknown, "Property", 5, 10);
            var validationErrors = new List<ValidationError> { err };

            OpenApi.Models.Result result = new OpenApi.Models.Result("/otherPath", "Post", NotFoundStatusCode, State.Fail, Reason.MissingMethod, validationErrors);

            List<OpenApi.Models.Result> resultList = new List<OpenApi.Models.Result>()
            {
                result
            };

            var subject = new JsonResultSerializer();

            var actual = subject.SerializeResult(resultList);

            Assert.Equal(FailJson, actual);
        }
    }
}
