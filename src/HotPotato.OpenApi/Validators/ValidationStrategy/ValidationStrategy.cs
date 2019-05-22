
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

            IValidationResult headerResult = HeaderValidator.Validate(StatusCodeValidator.Result);

            if (StatusCodeValidator.StatusCode == 204)
            {
                IValidationResult bodyResult = new ValidResult();
                AddValidationResult(bodyResult, headerResult);
            }
            else
            {
                IValidationResult bodyResult = BodyValidator.Validate(StatusCodeValidator.Result);
                AddValidationResult(bodyResult, headerResult);
            }
        }

        public void AddValidationResult(IValidationResult bodyResult, IValidationResult headerResult)
        {
            if (bodyResult.Valid && headerResult.Valid)
            {
                //only add one pass result
                AddPass();
            }
            else if (!bodyResult.Valid && headerResult.Valid)
            {
                //only add a failing result with no pass result
                InvalidResult invResult = (InvalidResult)bodyResult;
                AddFail(invResult.Reason, invResult.Errors);
            }
            else if (bodyResult.Valid && !headerResult.Valid)
            {
                InvalidResult invResult = (InvalidResult)headerResult;
                AddFail(invResult.Reason, invResult.Errors);
            }
            else
            {
                //TODO: Combine the two fail results, AUTOTEST-344
                InvalidResult invalidBody = (InvalidResult)bodyResult;
                InvalidResult invalidHeader = (InvalidResult)headerResult;

                AddFail(invalidBody.Reason, invalidBody.Errors);
                AddFail(invalidHeader.Reason, invalidHeader.Errors);
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
    }
}
