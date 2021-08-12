using HotPotato.Core.Http;
using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
	public interface IResultCollector
	{
		State OverallResult { get; }
		List<Result> Results { get; }
		void Pass(string path, string method, int statusCode, HttpHeaders customHeaders);
		void Fail(string path, string method, int statusCode, Reason[] reasons, HttpHeaders customHeaders, params ValidationError[] validationErrors);
	}
}
