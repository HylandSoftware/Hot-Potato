using HotPotato.Core.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HotPotato.AspNetCore.Host
{
	public class CookiesController : ControllerBase
	{
		private readonly ICookieJar _cookieJar;

		public CookiesController(ICookieJar cookieJar)
		{
			_cookieJar = cookieJar;
		}

		[HttpDelete]
		[Route("/cookies")]
		public IActionResult ExpireCookieContainer()
		{
			_cookieJar.ExpireCookies();

			return Ok();
		}
	}
}
