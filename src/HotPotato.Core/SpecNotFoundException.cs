﻿using System;
using System.Net.Http;

namespace HotPotato.Core
{
	public class SpecNotFoundException : Exception
	{
		public string SpecLocation { get; }
		public HttpResponseMessage Response { get; }

		public SpecNotFoundException(string specLocation, HttpResponseMessage response)
		{
			SpecLocation = specLocation;
			Response = response;
		}
	}
}
