using HotPotato.Http.Default;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MSHTTP = Microsoft.AspNetCore.Http;

namespace HotPotato.Http
{
    public static class HttpExtensions
    {
        private static readonly HashSet<string> MethodsWithPayload = new HashSet<string> { "POST", "PATCH", "PUT" };

        public static async Task<IHttpResponse> ConvertResponse(this HttpResponseMessage @this)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));

            HttpHeaders headers = new HttpHeaders();
            foreach (var item in @this.Headers)
            {
                headers.Add(item.Key, item.Value);
            }
            foreach (var item in @this.Content.Headers)
            {
                headers.Add(item.Key, item.Value);
            }
            byte[] payload = await @this.Content.ReadAsByteArrayAsync();
            return new HttpResponse(@this.StatusCode, headers, payload);
        }

        public static IHttpRequest ToRequest(this MSHTTP.HttpRequest @this, string remoteEndpoint)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));
            _ = remoteEndpoint ?? throw new ArgumentNullException(nameof(remoteEndpoint));

            HttpRequest request = new HttpRequest(new HttpMethod(@this.Method), @this.BuildUri(remoteEndpoint));
            if (MethodsWithPayload.Contains(@this.Method.ToUpperInvariant()) && @this?.Body?.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    @this.Body.CopyTo(stream);
                    request.SetContent(stream.ToArray(), @this.ContentType);
                } 
            }

            return request;
        }

        public static Uri BuildUri(this MSHTTP.HttpRequest @this, string remoteEndpoint)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));
            _ = remoteEndpoint ?? throw new ArgumentNullException(nameof(remoteEndpoint));

            return new Uri($"{remoteEndpoint}{@this.Path.Value}{@this.QueryString}");
        }

        public static async Task BuildResponse(this IHttpResponse @this, MSHTTP.HttpResponse response)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));
            _ = response ?? throw new ArgumentNullException(nameof(response));

            response.StatusCode = (int)@this.StatusCode;

            if (@this.Headers != null)
            {
                foreach (var header in @this.Headers)
                {
                    response.Headers.Add(header.Key, new StringValues(header.Value.ToArray()));
                }
            }

            if (@this.Content.Length > 0)
            {
                await response.Body.WriteAsync(@this.Content);
            }
        }
    }
}
