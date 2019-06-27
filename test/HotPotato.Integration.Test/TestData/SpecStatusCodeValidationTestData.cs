
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HotPotato.Http.Default
{
    public class StatusCodeNoContentTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "specs/document/", HttpMethod.Put,
            HttpStatusCode.NoContent, "http://api.docs.hyland.io/document/documents/56/keywords"};

            yield return new object[] { "specs/rdds/configurationservice/", HttpMethod.Patch,
            HttpStatusCode.NoContent, "https://api.hyland.com/ibpaf/rdds/configurations/93"};

            yield return new object[] { "specs/rdds/messagestorageservice/", HttpMethod.Delete,
            HttpStatusCode.NoContent, "https://api.hyland.com/ibpaf/rdds/messages/64"};

            yield return new object[] { "specs/rdds/onrampservice/", HttpMethod.Post,
            HttpStatusCode.NoContent, "https://api.hyland.com/ibpaf/rdds/notifications"};
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

