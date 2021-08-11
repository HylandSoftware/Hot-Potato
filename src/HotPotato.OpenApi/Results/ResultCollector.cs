using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public class ResultCollector : IResultCollector
    {
        public State OverallResult { get; private set; }
        public List<Result> Results { get; }

        public ResultCollector()
        {
            Results = new List<Result>();
            OverallResult = State.Inconclusive;
        }

        public void Pass(string path, string method, int statusCode, HttpHeaders customHeaders)
        {
            Results.Add(ResultFactory.PassResult(path, method, statusCode, customHeaders));
            //for if the body fails but the header passes
            if (OverallResult != State.Fail)
            {
                OverallResult = State.Pass;
            }
        }

        public void Fail(string path, string method, int statusCode, Reason[] reasons, HttpHeaders customHeaders, params ValidationError[] validationErrors)
        {
            Results.Add(ResultFactory.FailResult(path, method, statusCode, reasons, customHeaders, validationErrors));

            OverallResult = State.Fail;
        }
    }
}
