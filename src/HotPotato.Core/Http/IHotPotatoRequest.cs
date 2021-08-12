using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HotPotato.Core.Http
{
	public interface IHotPotatoRequest : IDisposable
	{
		HttpMethod Method { get; }
		Uri Uri { get; }
		string DecodedPath { get; }
		HttpHeaders HttpHeaders { get; }
		HttpHeaders CustomHeaders { get; }
		MediaTypeHeaderValue ContentType { get; }
		HttpContent Content { get; }
		IHotPotatoRequest SetContent(string content);
		IHotPotatoRequest SetContent(string content, Encoding encoding);
		IHotPotatoRequest SetContent(string content, Encoding encoding, string mediaType);
		IHotPotatoRequest SetContent(string content, Encoding encoding, MediaTypeHeaderValue mediaType);
	}
}
