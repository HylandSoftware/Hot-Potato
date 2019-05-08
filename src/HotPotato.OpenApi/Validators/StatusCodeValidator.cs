
using HotPotato.OpenApi.Models;
using NSwag;
using System;
using System.Net;

namespace HotPotato.OpenApi.Validators
{
    internal class StatusCodeValidator
    {
        public int statCode { get; }
        public string bodyString { get; }

        public Reason FailReason { get; private set; }
        public SwaggerResponse Result { get; private set; }

        public StatusCodeValidator(HttpStatusCode StatCode, string BodyString)
        {
            statCode = Convert.ToInt32(StatCode);
            bodyString = BodyString;
        }

        public bool Validate(SwaggerOperation swagOp)
        {
            string statCodeStr = statCode.ToString();
            if (swagOp.Responses.ContainsKey(statCodeStr))
            {
                if (statCodeStr == "204" && !string.IsNullOrWhiteSpace(bodyString))
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
