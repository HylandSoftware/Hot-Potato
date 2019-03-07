using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.Results
{
    public class ResultCollectorTest
    { 
        [Fact]
        public void CanIAddAPassResultToResultsList()
        {
            OpenApi.Models.Result expected = new OpenApi.Models.Result("/endpoint", "GET", 200, State.Pass);
            IHttpRequest req = new HttpRequest(HttpMethod.Get ,new Uri("http://localhost/endpoint"));
            IHttpResponse res = new HttpResponse(HttpStatusCode.OK, new HttpHeaders());
            HttpPair pair = new HttpPair(req, res);
            ResultCollector subject = new ResultCollector();

            subject.Pass(pair);

            Assert.NotEmpty(subject.resultList);
            Assert.Single(subject.resultList);
            Assert.Equal(expected.Path, subject.resultList[0].Path);
            Assert.Equal(expected.Method, subject.resultList[0].Method);
            Assert.Equal(expected.StatusCode, subject.resultList[0].StatusCode);
            Assert.Equal(expected.State, subject.resultList[0].State);
        }

        [Fact]
        public void CanIAddAFailResultToResultsList()
        {
            var err = new ValidationError("Error", ValidationErrorKind.Unknown, "Property", 5, 10);
            var validationErrors = new List<ValidationError> { err };
            OpenApi.Models.Result expected = new OpenApi.Models.Result("/endpoint", "GET", 404, State.Fail, Reason.Unknown, validationErrors);
            IHttpRequest req = new HttpRequest(HttpMethod.Get, new Uri("http://localhost/endpoint"));
            IHttpResponse res = new HttpResponse(HttpStatusCode.NotFound, new HttpHeaders());
            HttpPair pair = new HttpPair(req, res);
            ResultCollector subject = new ResultCollector();

            subject.Fail(pair, Reason.Unknown, validationErrors.ToArray());

            Assert.NotEmpty(subject.resultList);
            Assert.Single(subject.resultList);
            Assert.Equal(expected.Path, subject.resultList[0].Path);
            Assert.Equal(expected.Method, subject.resultList[0].Method);
            Assert.Equal(expected.StatusCode, subject.resultList[0].StatusCode);
            Assert.Equal(expected.State, subject.resultList[0].State);
            Assert.Equal(expected.Reason, subject.resultList[0].Reason);
            Assert.Equal(expected.ValidationErrors, subject.resultList[0].ValidationErrors);
        }
    }
}
