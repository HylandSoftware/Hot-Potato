namespace HotPotato.Core.Cookies
{
	public interface ICookieJar
	{
		System.Net.CookieContainer Cookies { get; }
		void ExpireCookies();
	}
}
