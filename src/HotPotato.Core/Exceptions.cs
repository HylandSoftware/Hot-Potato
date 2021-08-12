using System;
using System.Net.Http;

namespace HotPotato.Core
{
	public static class Exceptions
	{
		public static ArgumentNullException ArgumentNull(string paramName) =>
			new ArgumentNullException(paramName);
		public static InvalidOperationException InvalidOperation(string message) =>
			new InvalidOperationException(message);
		public static SpecNotFoundException SpecNotFound(string specLocation, HttpResponseMessage response) =>
			new SpecNotFoundException(specLocation, response);
	}
}
