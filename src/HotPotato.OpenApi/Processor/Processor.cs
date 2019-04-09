
using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using HotPotato.OpenApi.Validators;

namespace HotPotato.OpenApi.Processor
{
    public class Processor : IProcessor
    {
        private readonly IResultCollector collector;
        private readonly ISpecificationProvider specPro;

        public Processor(IResultCollector resColl, ISpecificationProvider specificationProvider)
        {
            collector = resColl;
            specPro = specificationProvider;
        }

        public void Process(HttpPair pair)
        {
            string body = pair.Response.ToBodyString();

            Validator val = new ValidationBuilder(collector, specPro)
                .WithPath(pair.Request.Uri.AbsolutePath)
                .WithMethod(pair.Request.Method)
                .WithStatusCode(pair.Response.StatusCode, body)
                .WithBody(body, pair.Response.ContentType.MediaType)
                .WithHeaders(pair.Response.Headers)
                .Build();

            val.Validate();
        }
    }
}
