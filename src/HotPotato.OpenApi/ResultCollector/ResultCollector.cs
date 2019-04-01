
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
            Results = new List<Models.Result>();
        }

        public void Pass(string path, string method, int statusCode)
        {
            Results.Add(ResultFactory.PassResult(path, method, statusCode, State.Pass));
        }
        public void Fail(string path, string method, int statusCode, Reason reason, params ValidationError[] validationErrors)
        {
            Results.Add(ResultFactory.FailResult(path, method, statusCode, State.Fail, reason, validationErrors));

            OverallResult = State.Fail;
        }
    }
}
