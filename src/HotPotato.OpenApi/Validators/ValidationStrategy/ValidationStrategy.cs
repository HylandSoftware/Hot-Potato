
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NSwag;
using System.Linq;

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

        private const int NoContentStatusCode = 204;

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

            if (StatusCodeValidator.StatusCode == NoContentStatusCode)
            {
                AddNoContentValidationResult(headerResult);
            }
            else
            {
                IValidationResult bodyResult = BodyValidator.Validate(StatusCodeValidator.Result);
                AddValidationResult(bodyResult, headerResult);
            }
        }

        internal void AddValidationResult(IValidationResult bodyResult, IValidationResult headerResult)
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
                InvalidResult invalidBody = (InvalidResult)bodyResult;
                InvalidResult invalidHeader = (InvalidResult)headerResult;

                AddFail(new Reason[2] { invalidBody.Reason, invalidHeader.Reason}, invalidBody.Errors?.Concat(invalidHeader.Errors).ToArray());
            }
        }

        /// <summary>
        /// Deal with HeaderValidation results after the body has been checked in StatusCodeValidator
        /// </summary>
        private void AddNoContentValidationResult(IValidationResult headerResult)
        {
            if (!headerResult.Valid)
            {
                InvalidResult invalidHeader = (InvalidResult)headerResult;
                AddFail(invalidHeader.Reason, invalidHeader.Errors);
            }
            else
            {
                AddPass();
            }
        }

        private void AddFail(Reason reason, params ValidationError[] validationErrors)
        {
            resColl.Fail(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode, new Reason[1] { reason }, validationErrors);
        }

        private void AddFail(Reason[] reasons, params ValidationError[] validationErrors)
        {
            resColl.Fail(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode, reasons, validationErrors);
        }

        private void AddPass()
        {
            resColl.Pass(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode);
        }
    }
}
