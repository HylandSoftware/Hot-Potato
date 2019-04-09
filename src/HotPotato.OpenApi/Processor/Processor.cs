
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
        private readonly ISpecificationProvider specificationProvider;

        public Processor(IResultCollector ResColl, ISpecificationProvider SpecificationProvider)
        {
            collector = ResColl;
            specificationProvider = SpecificationProvider;
        }

        public void Process(HttpPair pair)
        {
            Validator val = new ValidationBuilder(collector, specificationProvider)
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
