using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NJsonSchema;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class BodyValidator : IValidator
    {
        private readonly IValidationProvider valPro;
        private readonly IResultCollector collector;
        public BodyValidator(IValidationProvider valPro, IResultCollector collector)
        {
            this.valPro = valPro;
            this.collector = collector;
        }
        public void Validate(HttpPair pair)
        {
            SwaggerResponse swagResp = valPro.specResp;
            if (swagResp.ActualResponse == null || swagResp.ActualResponse.Schema == null)
            {
                collector.Fail(pair, Reason.MissingSpecBody);
            }
            JsonSchema4 specBody = valPro.specResp.ActualResponse.Schema;
            string respBodyString = pair.Response.ToBodyString();
            ICollection<NJsonSchema.Validation.ValidationError> errors = specBody.Validate(respBodyString);
            if (errors == null || errors.Count == 0)
            {
                collector.Pass(pair);
            }
            else
            {
                List<ValidationError> errList = errors.ToValidationErrorList();
                collector.Fail(pair, Reason.InvalidBody, errList.ToArray());


            }
        }
    }
}
