using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace HotPotato.Core.Http.Default
{
    public class HttpRequest : IHttpRequest, IDisposable
    {
        private readonly Encoding DefaultEncoding = Encoding.UTF8;
        private readonly string DefaultMediaType = "application/json";
        private HttpRequestMessage requestContent { get; }

        private bool disposed = false;

        public HttpMethod Method { get; }
        public Uri Uri { get; }
        /// <summary>
        /// Path with original special characters that are encoded automatically by Microsoft's Uri class
        /// </summary>
        public string DecodedPath { get; }
        public HttpHeaders HttpHeaders { get; }
        public HttpHeaders CustomHeaders { get; }
        public MediaTypeHeaderValue ContentType { get; private set; }
        public HttpContent Content => this.requestContent.Content;
        
        public HttpRequest(Uri uri)
            : this(HttpMethod.Get, uri)
        {
            this.Uri = uri;
            this.DecodedPath = HttpUtility.UrlDecode(uri.AbsolutePath);
        }

        public HttpRequest(HttpMethod method, Uri uri)
        {
            this.Method = method;
            this.Uri = uri;
            this.DecodedPath = HttpUtility.UrlDecode(uri.AbsolutePath);
            this.HttpHeaders = new HttpHeaders();
            this.CustomHeaders = new HttpHeaders();
            this.requestContent = new HttpRequestMessage();
        }

        public IHttpRequest SetContent(string content)
        {
            return SetContent(content, DefaultEncoding);
        }

        public IHttpRequest SetContent(string content, Encoding encoding)
        {
            return SetContent(content, encoding, DefaultMediaType);
        }

        public IHttpRequest SetContent(string content, Encoding encoding, string mediaType)
        {
            return SetContent(content, encoding, new MediaTypeHeaderValue(mediaType));
        }

        public IHttpRequest SetContent(string content, Encoding encoding, MediaTypeHeaderValue mediaType)
        {
            this.requestContent.Content = new StringContent(content, encoding, mediaType.MediaType);
            return this;
        }

        public IHttpRequest SetContent(byte[] content, string mediaType)
        {
            this.requestContent.Content = new ByteArrayContent(content);

            if (string.IsNullOrEmpty(mediaType))
            {
                this.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            }
            else
            {
                this.ContentType = new MediaTypeHeaderValue(mediaType);
            }

            return this;
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
                    requestContent.Dispose();
                }

                disposed = true;
            }
        }
    }
}
