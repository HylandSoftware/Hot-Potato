using System;
using HotPotato.Core.Models;
using HotPotato.Core.Processor;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.Validators;

namespace HotPotato.OpenApi.Processor
{
    internal class Processor : IProcessor
    {
        private readonly Func<string, IValidator> validationAccessor;
        private readonly IResultCollector collector;

        public Processor(Func<string, IValidator> valAccess, IResultCollector resColl)
        {
            validationAccessor = valAccess;
            collector = resColl;
        }

        public void Process(HttpPair pair)
        {
            ValidatePath(pair);
            if (HasValidationError())
            {
                return;
            }
            else
            {
                ValidateMethod(pair);
            }
            if (HasValidationError())
            {
                return;
            }
            else
            {
                ValidateStatusCode(pair);
            }
            if (HasValidationError())
            {
                return;
            }
            else
            {
                ValidateBody(pair);
                ValidateHeader(pair);
            }
        }

        public void ValidatePath(HttpPair pair)
        {
            validationAccessor("path").Validate(pair);
        }
        public void ValidateMethod(HttpPair pair)
        {
            validationAccessor("method").Validate(pair);
        }
        public void ValidateStatusCode(HttpPair pair)
        {
            validationAccessor("status").Validate(pair);
        }
        public void ValidateBody(HttpPair pair)
        {
            validationAccessor("body").Validate(pair);
        }
        public void ValidateHeader(HttpPair pair)
        {
            validationAccessor("header").Validate(pair);
        }
        public bool HasValidationError()
        {
            return (collector.Results.Count > 0) ? true : false;
        }
    }
}
