
using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using HotPotato.OpenApi.SpecificationProvider;
using NJsonSchema;
using NSwag;
using System.Linq;

namespace HotPotato.OpenApi.Validators
{
    public class ValidationStrategy : IValidationStrategy
    {
        internal PathValidator PathValidator { get; set; }
        internal MethodValidator MethodValidator { get; set; }
        internal StatusCodeValidator StatusCodeValidator { get; set; }
        internal ContentValidator ContentValidator { get; set; }
        internal BodyValidator BodyValidator { get; set; }
        internal HeaderValidator HeaderValidator { get; set; }
        internal HttpHeaders CustomHeaders { get; set; }

        private IResultCollector resColl { get; }
        private OpenApiDocument swagDoc { get; }
        private HttpContentType contentType { get; }

        public ValidationStrategy(IResultCollector ResColl, ISpecificationProvider SpecPro, HttpContentType ContentType)
        {
            resColl = ResColl;
            swagDoc = SpecPro.GetSpecDocument();
            contentType = ContentType;
        }

        public void Validate()
        {
            if (!PathValidator.Validate(swagDoc))
            {
                AddFail(Reason.MissingPath);
                return;
            }
            if (!MethodValidator.Validate(PathValidator.Result))
            {
                AddFail(Reason.MissingMethod);
                return;
            }
            if (!StatusCodeValidator.Validate(MethodValidator.Result))
            {
                AddFail(StatusCodeValidator.FailReason);
                return;
            }

            JsonSchema schema = ContentProvider.GetSchema(StatusCodeValidator.Result, contentType.Type);

            IValidationResult bodyResult = ContentValidator.Validate(schema);

            if (bodyResult == null)
            {
                bodyResult = BodyValidator.Validate(schema);
            }

            IValidationResult headerResult = HeaderValidator.Validate(StatusCodeValidator.Result);

            AddValidationResult(bodyResult, headerResult);
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

                AddFail(new Reason[2] { invalidBody.Reason, invalidHeader.Reason }, invalidBody.Errors?.Concat(invalidHeader.Errors).ToArray());
            }
        }

        private void AddFail(Reason reason, params ValidationError[] validationErrors)
        {
            resColl.Fail(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode, new Reason[1] { reason }, CustomHeaders, validationErrors);
        }

        private void AddFail(Reason[] reasons, params ValidationError[] validationErrors)
        {
            resColl.Fail(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode, reasons, CustomHeaders, validationErrors);
        }

        private void AddPass()
        {
            resColl.Pass(PathValidator.Path, MethodValidator.Method, StatusCodeValidator.StatusCode, CustomHeaders);
        }
    }
}
