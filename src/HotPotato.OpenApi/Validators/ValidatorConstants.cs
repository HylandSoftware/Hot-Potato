using System.Collections.Generic;
using System.Collections.Immutable;

namespace HotPotato.OpenApi.Validators
{
    public static class ValidatorConstants
    {
        //204 No Content: https://tools.ietf.org/html/rfc7231#section-6.3.5
        //The 204 (No Content) status code indicates that the server has
        //successfully fulfilled the request and that there is no additional
        //content to send in the response payload body.

        //205 Reset Content: https://tools.ietf.org/html/rfc7231#section-6.3.6
        //Since the 205 status code implies that no additional content will be
        //provided, a server MUST NOT generate a payload in a 205 response.

        //304 Not Modified: https://tools.ietf.org/html/rfc7232#section-4.1
        //A 304 response cannot contain a message-body; it is always terminated
        //by the first empty line after the header fields.

        /// <summary>
        /// Currently determined list of status codes that must not have a response body
        /// </summary>
        public static readonly ImmutableList<string> NoContentStatusCodes = new List<string>{ "204", "205", "304" }.ToImmutableList();

    }
}
