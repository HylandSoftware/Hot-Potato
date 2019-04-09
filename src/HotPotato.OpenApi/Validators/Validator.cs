
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    public class Validator
    {
        internal PathValidator pathVal { get; set; }
        internal MethodValidator methodVal { get; set; }
        internal StatusCodeValidator statusCodeVal { get; set; }
        internal BodyValidator bodyVal { get; set; }
        internal HeaderValidator headerVal { get; set; }

        private IResultCollector resColl { get; }
        private SwaggerDocument swagDoc { get; }

        public Validator(IResultCollector ResColl, ISpecificationProvider SpecPro)
        {
            resColl = ResColl;
            swagDoc = SpecPro.GetSpecDocument();
        }

        public void Validate()
        {
            if (!pathVal.Validate(swagDoc))
            {
                AddFail(Reason.MissingPath);
                return;
            }
            if(!methodVal.Validate(pathVal.Result))
            {
                AddFail(Reason.MissingMethod);
                return;
            }
            if(!statusCodeVal.Validate(methodVal.Result))
            {
                AddFail(statusCodeVal.FailReason);
                return;
            }
            else if(statusCodeVal.statCode == 204)
            {
                AddPass();
                return;
            }

            if (!bodyVal.Validate(statusCodeVal.Result))
            {
                AddFail(bodyVal.FailReason, bodyVal.ErrorArr);
            }
            else
            {
                AddPass();
            }
            if (!headerVal.Validate(statusCodeVal.Result))
            {
                AddFail(headerVal.FailReason, headerVal.ErrorArr);
            }
            else
            {
                AddPass();
            }
        }

        public void AddFail(Reason reason, params ValidationError[] validationErrors)
        {
            resColl.Fail(pathVal.path, methodVal.method, statusCodeVal.statCode, reason, validationErrors);
        }
        
        public void AddPass()
        {
            resColl.Pass(pathVal.path, methodVal.method, statusCodeVal.statCode);
        }
    }
}
