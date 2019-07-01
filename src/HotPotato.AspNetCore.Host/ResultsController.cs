using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace HotPotato.AspNetCore.Host
{
    public class ResultsController : Controller
    {
        private IResultCollector _resultCollector;
        private ILogger _logger;

        public ResultsController(IResultCollector rc, ILogger<ResultsController> logger)
        {
            _resultCollector = rc;
            _logger = logger;
        }

        [HttpGet]
        [Route("/results")]
        public IActionResult Get()
        { 
            _logger.LogDebug("Getting Results...");

            HttpContext.Response.Headers.Add("X-Status", _resultCollector.OverallResult.ToString());

            var res = new List<Result>();

            foreach (var item in _resultCollector.Results)
            {
                res.Add(item);
            }

            _resultCollector.Results.Clear();

            return Ok(res);
        }
    }
}
