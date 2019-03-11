using HotPotato.Core.Models;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public class ResultCollector : IResultCollector
    {
        public List<Models.Result> resultList = new List<Models.Result>();

        public void Pass(HttpPair pair)
        {
            resultList.Add(ResultFactory.PassResult(pair.Request.Uri.AbsolutePath, pair.Request.Method.ToString(), (int)pair.Response.StatusCode, State.Pass));
        }

        public void Fail(HttpPair pair, Reason reason, params ValidationError[] validationErrors)
        {
            resultList.Add(ResultFactory.FailResult(pair.Request.Uri.AbsolutePath, pair.Request.Method.ToString(), (int)pair.Response.StatusCode, State.Fail, reason, validationErrors));
        }
    }
}
