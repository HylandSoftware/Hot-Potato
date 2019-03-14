﻿
using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NSwag;
using System;

namespace HotPotato.OpenApi.Validators
{
    internal class StatusCodeValidator : IValidator
    {
        private readonly IValidationProvider valPro;
        private readonly IResultCollector collector;
        public StatusCodeValidator(IValidationProvider valPro, IResultCollector collector)
        {
            this.valPro = valPro;
            this.collector = collector;
        }
        public void Validate(HttpPair pair)
        {
            _ = pair ?? throw new ArgumentNullException(nameof(pair));

            string statusCode = Convert.ToInt32(pair.Response.StatusCode).ToString();
            SwaggerOperation swagOp = valPro.specMeth;
            if (swagOp.Responses.ContainsKey(statusCode))
            {
                if (statusCode == "204")
                {
                    if (swagOp.Responses[statusCode].Content != null)
                    {
                        collector.Pass(pair);
                    }
                    collector.Fail(pair, Reason.UnexpectedBody);
                }
                valPro.specResp = swagOp.Responses[statusCode];
            }
            else
            {
                collector.Fail(pair, Reason.MissingStatusCode);
            }
        }
    }
}
