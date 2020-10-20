using HotPotato.Core;
using Microsoft.Extensions.Configuration;
using NSwag;
using static NSwag.SwaggerYamlDocument;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class SpecificationProvider : ISpecificationProvider
    {
        private readonly string SpecLocation;
        private readonly bool ignoreClientCertificateValidationErrors;

        public SpecificationProvider(IConfiguration config)
        {
            _ = config ?? throw Exceptions.ArgumentNull(nameof(config));
            this.SpecLocation = config["SpecLocation"];
            //mirror the security setting used at startup
            this.ignoreClientCertificateValidationErrors = config.GetSection("HttpClientSettings").GetValue<bool>("IgnoreClientHttpsCertificateValidationErrors");
        }
        public SwaggerDocument GetSpecDocument()
        {
            Task<SwaggerDocument> swagTask = null;
            if (Path.IsPathFullyQualified(SpecLocation))
            {
                swagTask = FromFileAsync(SpecLocation);
            }
            else if (Uri.IsWellFormedUriString(SpecLocation, UriKind.Absolute))
            {
                swagTask = FromUrlAsyncWithClient(SpecLocation);
            }
            else
            {
                throw Exceptions.InvalidOperation("AppSettings does not contain a valid Spec Location");
            }
            return swagTask.Result;
        }

        private async Task<SwaggerDocument> FromUrlAsyncWithClient(string url)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                if (ignoreClientCertificateValidationErrors)
                {
                    //both of these need to be set to avoid the SSL connection error
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, errors) => true);
                    handler.ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) =>
                    {
                        //output which certificates are being accepted
                        Console.WriteLine(certificate);
                        return true;
                    };
                    //this is a possible fix for an issue that was causing requests to be forcibly closed
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                }
                using (HttpClient client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(url);
                    using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        using (HttpResponseMessage response = await client.SendAsync(req).ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            return await FromYamlAsync(data, url).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}
