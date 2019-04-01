
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
            Validator val = new ValidationBuilder(collector, specPro)
                .WithPath(pair.Request.Uri.AbsolutePath)
                .WithMethod(pair.Request.Method)
                .WithStatusCode(pair.Response.StatusCode, pair.Response.ToBodyString())
                .WithBody(pair.Response.ToBodyString())
                .WithHeaders(pair.Response.Headers)
                .Build();

            val.Validate();
        }
    }
}
