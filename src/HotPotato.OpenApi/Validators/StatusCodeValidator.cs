
using HotPotato.OpenApi.Models;
using NSwag;
using System;
using System.Net;

namespace HotPotato.OpenApi.Validators
{
    internal class StatusCodeValidator
    {
        public int statCode;
        public string bodyString;

        public Reason FailReason;
        public SwaggerResponse Result;

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
                if (statCodeStr == "204")
                {
                    if (string.IsNullOrWhiteSpace(bodyString))
                    {
                        return true;
                    }
                    else
                    {
                        FailReason = Reason.UnexpectedBody;
                        return false;
                    }
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
