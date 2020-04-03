
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
        private readonly IResultCollector ResultCollector;
        private readonly ISpecificationProvider SpecificationProvider;

        public Processor(IResultCollector resColl, ISpecificationProvider specPro)
        {
            ResultCollector = resColl;
            SpecificationProvider = specPro;
        }

        public void Process(HttpPair pair)
        {
            IValidationStrategy val = new ValidationBuilder(ResultCollector, SpecificationProvider)
                .WithPath(pair.Request.DecodedPath)
                .WithMethod(pair.Request.Method)
                .WithStatusCode(pair.Response.StatusCode)
                .WithBody(pair.Response.ToBodyString(), pair.Response.ContentType)
                .WithHeaders(pair.Response.Headers)
                .WithCustomHeaders(pair.Request.CustomHeaders)
                .Build();

            val.Validate();
        }
    }
}
