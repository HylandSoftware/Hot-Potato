using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Http.Default
{
    internal class HttpClient : IHttpClient
    {
        public async Task<IHttpResponse> SendAsync(IHttpRequest request)
        {
            HttpRequestMessage message = new HttpRequestMessage(request.Method, request.Uri);
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
