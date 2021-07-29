using System;
using System.Net.Http;
using Xunit;

namespace HotPotato.Core
{
    public class ExceptionsTest
    {
        private const string AValidParam = "Param";
        private const string AValidMessage = "Message";
        private const string AValidSpecLocation = "https://raw.githubusercontent.com/HylandSoftware/Hot-Potato/master/test/RawPotatoSpec.yaml";

        private readonly Type ArgumentNullExceptionType = typeof(ArgumentNullException);
        private readonly Type InvalidOperationExceptionType = typeof(InvalidOperationException);
        private readonly Type SpecNotFoundExceptionType = typeof(SpecNotFoundException);

        [Fact]
        public void ArgumentNull_ReturnsArgumentNullException()
        {
            var result = Exceptions.ArgumentNull(AValidParam);

            Assert.IsType(ArgumentNullExceptionType, result);
            Assert.Equal(AValidParam, result.ParamName);
        }

        [Fact]
        public void InvalidOperation_ReturnsInvalidOperationException()
        {
            var result = Exceptions.InvalidOperation(AValidMessage);

            Assert.IsType(InvalidOperationExceptionType, result);
            Assert.Equal(AValidMessage, result.Message);
        }

        [Fact]
        public void SpecNotFound_ReturnsSpecNotFoundException()
		{
            HttpResponseMessage ADefaultResponse = new HttpResponseMessage();

            var result = Exceptions.SpecNotFound(AValidSpecLocation, ADefaultResponse);

            Assert.IsType(SpecNotFoundExceptionType, result);
            Assert.Equal(AValidSpecLocation, result.SpecLocation);
            Assert.Equal(ADefaultResponse, result.Response);
		}
    }
}
