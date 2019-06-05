using HotPotato.Core.Http;
using System;

namespace HotPotato.Core.Models
{
    public class HttpPair : IDisposable
    {
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }
        private bool disposed = false;
        public HttpPair(IHttpRequest request, IHttpResponse response)
        {
            _ = request ?? throw Exceptions.ArgumentNull(nameof(request));
            _ = response ?? throw Exceptions.ArgumentNull(nameof(response));

            this.Request = request;
            this.Response = response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Request.Dispose();
                }

                disposed = true;
            }
        }
    }
}
