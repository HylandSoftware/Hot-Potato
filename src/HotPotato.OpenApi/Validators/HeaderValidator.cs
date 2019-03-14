using HotPotato.Core.Http;
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using Newtonsoft.Json;
using NSwag;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Validators
{
    internal class HeaderValidator : IValidator
    {
        private readonly IValidationProvider valPro;
        private readonly IResultCollector collector;
        public HeaderValidator(IValidationProvider valPro, IResultCollector collector)
        {
            this.valPro = valPro;
            this.collector = collector;
        }

        public void Validate (HttpPair pair)
        {
            SwaggerResponse swagResp = valPro.specResp;
            HttpHeaders pairRespHeaders = pair.Response.Headers;

            if (swagResp.Headers != null && swagResp.Headers.Count > 0)
            {
                foreach (var item in swagResp.Headers)
                {
                    string headerKey = item.Key;
                    if (pairRespHeaders == null || !pairRespHeaders.ContainsKey(headerKey))
                    {
                        collector.Fail(pair, Reason.MissingHeaders);
                        return;
                    }
                    else
                    {
                        List<string> headerValues = pairRespHeaders[headerKey];
                        foreach (string value in headerValues)
                        {
                            // HACK - Need to convert to JSON because that's how NJsonSchema likes it.
                            string jValue = JsonConvert.SerializeObject(value);
                            ICollection<NJsonSchema.Validation.ValidationError> errors = item.Value.Validate(jValue);
                            if (errors == null || errors.Count == 0)
                            {
                                collector.Pass(pair);
                            }
                            else
                            {
                                List<ValidationError> errList = errors.ToValidationErrorList();
                                collector.Fail(pair, Reason.InvalidHeaders, errList.ToArray());
                            }
                        }

                    }
                }
            }
        }

    }
}
