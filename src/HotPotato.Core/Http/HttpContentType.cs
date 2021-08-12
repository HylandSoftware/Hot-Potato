
namespace HotPotato.Core.Http
{
	public class HttpContentType
	{
		private const string DefaultCharSet = "utf-8";

		public string Type { get; }
		public string CharSet { get; }

		public HttpContentType(string type, string charSet = DefaultCharSet)
		{
			Type = type;
			CharSet = charSet;
		}
	}
}
