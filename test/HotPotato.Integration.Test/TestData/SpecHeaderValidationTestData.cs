
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HotPotato.Http.Default
{
	public class SpecHeaderTestData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] { "specs/document/", HttpMethod.Post,
				HttpStatusCode.Created, "http://api.docs.hyland.io/document/documents/", "application/json", new {
					id = "string"
				}
			};
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public class SpecHeaderWithExpectedNoContentTestData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] { "specs/rdds/messagestorageservice/", HttpMethod.Post,
			HttpStatusCode.Created, "https://api.hyland.com/ibpaf/rdds/messages"};
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public class SpecHeaderWithUnexpectedContentTestData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] { "specs/rdds/messagestorageservice/", HttpMethod.Post,
			HttpStatusCode.Created, "https://api.hyland.com/ibpaf/rdds/messages", "text/plain", "an unexpected body" };

			yield return new object[] { "specs/rdds/messagestorageservice/", HttpMethod.Post,
			HttpStatusCode.Created, "https://api.hyland.com/ibpaf/rdds/messages", "application/json",
				"{body = \"unexpected\"}" };

			yield return new object[] { "specs/rdds/messagestorageservice/", HttpMethod.Post,
			HttpStatusCode.Created, "https://api.hyland.com/ibpaf/rdds/messages", "application/json",
				@"<Body>Unexpected</Body>" };
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

