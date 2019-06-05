
using HotPotato.OpenApi.Models;
using NSwag;
using System;
using System.Net;

namespace HotPotato.OpenApi.Validators
{
    internal class StatusCodeValidator
    {
        public int StatusCode { get; }
        public string BodyString { get; }

        public Reason FailReason { get; private set; }
        public SwaggerResponse Result { get; private set; }

        private const string NoContentStatusCode = "204";

        public StatusCodeValidator(HttpStatusCode statCode, string bodyString)
        {
            StatusCode = Convert.ToInt32(statCode);
            BodyString = bodyString;
        }

        public bool Validate(SwaggerOperation swagOp)
        {
            string statCodeStr = StatusCode.ToString();
            if (swagOp.Responses.ContainsKey(statCodeStr))
            {
                if (statCodeStr == NoContentStatusCode && !string.IsNullOrWhiteSpace(BodyString))
                {
                    FailReason = Reason.UnexpectedBody;
                    return false;
                }
                Result = swagOp.Responses[statCodeStr];
                return true;
            }
            else
            {
                FailReason = Reason.MissingStatusCode;
                return false;
            }
        }
    }
}
