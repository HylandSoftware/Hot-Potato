using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Http.Default
{
    internal class HttpClient : IHttpClient
    {
        public Task<IHttpResponse> Delete(IHttpRequest request)
        {
            return this.SendAsync(request, HttpMethod.Delete);
        }

        public Task<IHttpResponse> Get(IHttpRequest request)
        {
            return this.SendAsync(request, HttpMethod.Get);
        }

        public Task<IHttpResponse> Options(IHttpRequest request)
        {
            return this.SendAsync(request, HttpMethod.Options);
        }

        public Task<IHttpResponse> Patch(IHttpRequest request)
        {
            return this.SendAsync(request, HttpMethod.Patch);
        }

        public Task<IHttpResponse> Post(IHttpRequest request)
        {
            return this.SendAsync(request, HttpMethod.Post);
        }

        public  Task<IHttpResponse> Put(IHttpRequest request)
        {
            return this.SendAsync(request, HttpMethod.Put);
        }

        private async Task<IHttpResponse> SendAsync(IHttpRequest request, HttpMethod method)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, request.Uri);
            message.Content = request.Content;
            foreach (var item in request.HttpHeaders)
            {
                if (!message.Headers.TryAddWithoutValidation(item.Key, item.Value))
                {
                    message.Content.Headers.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

            using (System.Net.Http.HttpClient client = GetHttpClient())
            {
                using (HttpResponseMessage response = await client.SendAsync(message))
                {
                    return await response.ConvertResponse();
                }
            }
        }

        private System.Net.Http.HttpClient GetHttpClient()
        {
            return new System.Net.Http.HttpClient(new HttpClientHandler());
        }
    }
}
