﻿using HotPotato.Core.Http;
using Moq;
using System;
using Xunit;

namespace HotPotato.Core.Models
{
    public class HttpPairTest
    {

        [Fact]
        public void HttpPair_Constructor_ThrowsArgumentNullExceptionWithRequest()
        {
            Action subject = () => new HttpPair(null, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpPair_Constructor_ThrowsArgumentNullExceptionWithResponse()
        {
            IHttpRequest request = Mock.Of<IHttpRequest>();
            Action subject = () => new HttpPair(request, null);
            Assert.Throws<ArgumentNullException>(subject);
        }

        [Fact]
        public void HttpPair_Constructor_SetsRequestandResponse()
        {
            IHttpRequest request = Mock.Of<IHttpRequest>();
            IHttpResponse response = Mock.Of<IHttpResponse>();

            HttpPair subject = new HttpPair(request, response);

            Assert.Equal(request, subject.Request);
            Assert.Equal(response, subject.Response);
        }
    }
}