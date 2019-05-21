
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NSwag;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationStrategy : IValidationStrategy
    {
        internal PathValidator PathValidator { get; set; }
        internal MethodValidator MethodValidator { get; set; }
        internal StatusCodeValidator StatusCodeValidator { get; set; }
        internal BodyValidator BodyValidator { get; set; }
        internal HeaderValidator HeaderValidator { get; set; }

        private IResultCollector resColl { get; }
        private SwaggerDocument swagDoc { get; }

        public ValidationStrategy(IResultCollector ResColl, ISpecificationProvider SpecPro)
        {
            resColl = ResColl;
            swagDoc = SpecPro.GetSpecDocument();
        }

        public void Validate()
        {
            if (!PathValidator.Validate(swagDoc))
            {
                AddFail(Reason.MissingPath);
                return;
            }
            if(!MethodValidator.Validate(PathValidator.Result))
            {
                AddFail(Reason.MissingMethod);
                return;
            }
            if(!StatusCodeValidator.Validate(MethodValidator.Result))
            {
                AddFail(StatusCodeValidator.FailReason);
                return;
            }
            else if(StatusCodeValidator.StatusCode == 204)
            {
                AddValidationResult(HeaderValidator.Validate(StatusCodeValidator.Result));
                return;
            }

            IValidationResult bodyResult = BodyValidator.Validate(StatusCodeValidator.Result);
            AddValidationResult(bodyResult);

            IValidationResult headerResult = HeaderValidator.Validate(StatusCodeValidator.Result);
            //BodyValidator and HeaderValidator return identical pass results,
            //only want to add both if one or both fails
            if (!bodyResult.Valid || !headerResult.Valid)
            {
                AddValidationResult(headerResult);
            }
        }

        private void AddFail(Reason reason, params ValidationError[] validationErrors)
        {
            resColl.Fail(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode, reason, validationErrors);
        }
        
        private void AddPass()
        {
            resColl.Pass(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode);
        }

        private void AddValidationResult(IValidationResult result)
        {
            if (result.Valid)
            {
                AddPass();
            }
            else
            {
                InvalidResult invResult = (InvalidResult)result;
                AddFail(invResult.Reason, invResult.Errors);
            }
        }
    }
}
