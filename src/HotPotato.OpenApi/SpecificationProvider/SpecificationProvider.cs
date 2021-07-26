﻿using HotPotato.Core;
using Microsoft.Extensions.Configuration;
using NSwag;
using static NSwag.OpenApiYamlDocument;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class SpecificationProvider : ISpecificationProvider
    {
        private readonly string SpecLocation;
        private readonly bool ignoreClientCertificateValidationErrors;
        private ILogger Logger;

        public SpecificationProvider(IConfiguration config, ILogger<SpecificationProvider> logger)
        {
            _ = config ?? throw Exceptions.ArgumentNull(nameof(config));
            this.SpecLocation = config["SpecLocation"];
            //mirror the security setting used at startup
            this.ignoreClientCertificateValidationErrors = config.GetSection("HttpClientSettings").GetValue<bool>("IgnoreClientHttpsCertificateValidationErrors");
        }
        public OpenApiDocument GetSpecDocument()
        {
            Console.WriteLine(SpecLocation);
            Task<OpenApiDocument> swagTask = null;
            if (Path.IsPathRooted(SpecLocation))
			{
                if (Path.IsPathFullyQualified(SpecLocation))
				{
                    swagTask = FromFileAsync(SpecLocation);
                }
                else
				{
                    swagTask = FromFileAsync(RelativeSpecLocationFullPath());
				}
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

        private string RelativeSpecLocationFullPath()
		{
            DirectoryInfo directory = Directory.GetParent(Environment.CurrentDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return Path.Join(directory.FullName, SpecLocation);
		}

        private async Task<OpenApiDocument> FromUrlAsyncWithClient(string url)
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
