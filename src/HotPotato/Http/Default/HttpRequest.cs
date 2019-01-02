using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HotPotato.Http.Default
{
    internal class HttpRequest : IHttpRequest
    {
        private readonly Encoding DefaultEncoding = Encoding.UTF8;
        private readonly string DefaultMediaType = "application/json";
        private HttpRequestMessage requestContent { get; }

        public HttpMethod Method { get; }
        public Uri Uri { get; set; }
        public HttpHeaders HttpHeaders { get; set; }
        public MediaTypeHeaderValue ContentType { get; set; }
        public HttpContent Content => this.requestContent.Content;

        public HttpRequest()
        {
            this.HttpHeaders = new HttpHeaders();
            this.requestContent = new HttpRequestMessage();
        }

        public HttpRequest(Uri uri)
            : this()
        {
            this.Uri = uri;
        }

        public HttpRequest(HttpMethod method, Uri uri)
            : this(uri)
        {
            Method = method;
        }

        public HttpRequest(string uri)
            : this(new Uri(uri))
        { }

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
    }
}
