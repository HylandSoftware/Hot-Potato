using HotPotato.OpenApi.Models;
using HotPotato.Core.Http;
using HotPotato.OpenApi.Validators;
using System.Linq;

namespace HotPotato.OpenApi.Results
{
	public static class ResultFactory
	{
		public static Result PassResult(string path, string method, int statusCode, HttpHeaders customHeaders)
		{
			if (customHeaders == null || customHeaders.Count == 0)
			{
				return new PassResult(path, method, statusCode);
			}
			else
			{
				return new PassResultWithCustomHeaders(path, method, statusCode, customHeaders);
			}
		}
		public static Result FailResult(string path, string method, int statusCode, Reason[] reasons, HttpHeaders customHeaders, params ValidationError[] validationErrors)
		{
			if (customHeaders == null || customHeaders.Count == 0)
			{
				return new FailResult(path, method, statusCode, reasons?.ToList(), validationErrors?.ToList());
			}
			else
			{
				return new FailResultWithCustomHeaders(path, method, statusCode, reasons?.ToList(), customHeaders, validationErrors?.ToList());
			}
		}
	}
}
