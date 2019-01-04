﻿using HotPotato.Http.Default;
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
            if (@this.Headers != null)
            {
                foreach (var item in @this?.Headers)
                {
                    headers.Add(item.Key, item.Value);
                }
            }
            if (@this.Content != null)
            {
                foreach (var item in @this.Content?.Headers)
                {
                    headers.Add(item.Key, item.Value);
                }
                byte[] payload = await @this.Content.ReadAsByteArrayAsync();
                return new HttpResponse(@this.StatusCode, headers, payload);
            }
            return new HttpResponse(@this.StatusCode, headers);
        }

        public static IHttpRequest ToRequest(this MSHTTP.HttpRequest @this, string remoteEndpoint)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));
            _ = remoteEndpoint ?? throw new ArgumentNullException(nameof(remoteEndpoint));

            HttpRequest request = new HttpRequest(new HttpMethod(@this.Method), @this.BuildUri(remoteEndpoint));
            if (MethodsWithPayload.Contains(@this.Method.ToUpperInvariant()) && @this.Body != null)
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

        public static HttpRequestMessage BuildRequest(this IHttpRequest @this)
        {
            _ = @this ?? throw new ArgumentNullException(nameof(@this));

            HttpRequestMessage message = new HttpRequestMessage(@this.Method, @this.Uri);
            if (@this.Content != null)
            {
                message.Content = @this.Content; 
            }
            foreach (var item in @this.HttpHeaders)
            {
                if (!message.Headers.TryAddWithoutValidation(item.Key, item.Value))
                {
                    message.Content.Headers.TryAddWithoutValidation(item.Key, item.Value);
                }
            }
            return message;
        }
    }
}
