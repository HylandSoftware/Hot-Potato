using HotPotato.Core.Http;
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
            ValidationStrategy result = (ValidationStrategy)subject.WithPath(expected).Build();

            Assert.Equal(expected, result.PathValidator.Path);
        }
        [Fact]
        public void Builder_ReturnsValWithMethod()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            ValidationStrategy result = (ValidationStrategy)subject.WithMethod(HttpMethod.Trace).Build();

            Assert.Equal("trace", result.MethodValidator.Method);
        }
        [Fact]
        public void Builder_ReturnsValWithStatusCode()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            HttpStatusCode expectedStatCode = HttpStatusCode.Ambiguous;
            string expectedBody = "{'perfectSquare': '16'}";
            ValidationStrategy result = (ValidationStrategy)subject.WithStatusCode(expectedStatCode).WithBody(expectedBody, new HttpContentType("application/json")).Build();

            Assert.Equal(Convert.ToInt32(expectedStatCode), result.StatusCodeValidator.StatusCode);
            Assert.Equal(expectedBody, result.StatusCodeValidator.BodyString);
        }
        [Fact]
        public void Builder_ReturnsValWithBody()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            string expected = "{'perfectSquare': '64'}";
            ValidationStrategy result = (ValidationStrategy)subject.WithBody(expected, new HttpContentType("application/json")).Build();

            Assert.Equal(expected, result.BodyValidator.BodyString);
        }
        [Fact]
        public void Builder_ReturnsValWithHeaders()
        {
            ValidationBuilder subject = new ValidationBuilder(Mock.Of<IResultCollector>(), Mock.Of<ISpecificationProvider>());

            Core.Http.HttpHeaders expected = new Core.Http.HttpHeaders();
            expected.Add("perfectSquare", "36");
            ValidationStrategy result = (ValidationStrategy)subject.WithHeaders(expected).Build();

            Assert.Equal(expected, result.HeaderValidator.Headers);
        }
    }
}
