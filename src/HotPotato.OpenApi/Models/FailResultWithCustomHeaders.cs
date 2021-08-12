using HotPotato.Core.Http;
using HotPotato.OpenApi.Validators;
using System.Collections.Generic;
using System.Linq;

namespace HotPotato.OpenApi.Models
{
	public class FailResultWithCustomHeaders : FailResult
	{
		/// <summary>
		/// User-defined custom variables
		/// </summary>
		public Dictionary<string, List<string>> Custom { get; }

		public FailResultWithCustomHeaders(string path, string method, int statusCode, List<Reason> reasons, HttpHeaders customHeaders, List<ValidationError> validationErrors) :
			base(path, method, statusCode, reasons, validationErrors)
		{
			Custom = customHeaders.ToDictionary(x => x.Key, y => y.Value);
		}
	}
}
