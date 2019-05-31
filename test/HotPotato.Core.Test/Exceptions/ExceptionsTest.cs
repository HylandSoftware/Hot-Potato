using System;
using Xunit;

namespace HotPotato.Core
{
    public class ExceptionsTest
    {
        private const string AValidParam = "Param";
        private const string AValidMessage = "Message";

        private readonly Type ArgumentNullExceptionType = typeof(ArgumentNullException);
        private readonly Type InvalidOperationExceptionType = typeof(InvalidOperationException);
        private readonly Type NotImplementedExceptionType = typeof(NotImplementedException);

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
        public void NotImplemented_ReturnsNotImplementedExceptionType()
        {
            var result = Exceptions.NotImplemented(AValidMessage);

            Assert.IsType(NotImplementedExceptionType, result);
            Assert.Equal(AValidMessage, result.Message);
        }
    }
}
