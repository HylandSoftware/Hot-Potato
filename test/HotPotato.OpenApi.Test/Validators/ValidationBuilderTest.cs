
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationBuilderTest
    {
        [Fact]
        public void Builder_ReturnsValWithPath()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            string expected = "deficiencies/deficiencies";
            Validator result = subject.WithPath(expected).Build();

            Assert.Equal(expected, result.pathVal.path);
        }
        [Fact]
        public void Builder_ReturnsValWithMethod()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            Validator result = subject.WithMethod(HttpMethod.Trace).Build();

            Assert.Equal("trace", result.methodVal.method);
        }
        [Fact]
        public void Builder_ReturnsValWithStatusCode()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            HttpStatusCode expectedStatCode = HttpStatusCode.Ambiguous;
            string expectedBody = "{'perfectSquare': '16'}";
            Validator result = subject.WithStatusCode(expectedStatCode).WithBody(expectedBody, "application/json").Build();

            Assert.Equal(Convert.ToInt32(expectedStatCode), result.statusCodeVal.statCode);
            Assert.Equal(expectedBody, result.statusCodeVal.bodyString);
        }
        [Fact]
        public void Builder_ReturnsValWithBody()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            string expected = "{'perfectSquare': '64'}";
            Validator result = subject.WithBody(expected, "application/json").Build();

            Assert.Equal(expected, result.bodyVal.bodyString);
        }
        [Fact]
        public void Builder_ReturnsValWithHeaders()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            Core.Http.HttpHeaders expected = new Core.Http.HttpHeaders();
            expected.Add("perfectSquare", "36");
            Validator result = subject.WithHeaders(expected).Build();

            Assert.Equal(expected, result.headerVal.headers);
        }
    }
}
