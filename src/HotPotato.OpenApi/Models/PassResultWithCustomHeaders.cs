using HotPotato.Core.Http;
using System.Collections.Generic;
using System.Linq;

namespace HotPotato.OpenApi.Models
{
    public class PassResultWithCustomHeaders : PassResult
    {
        /// <summary>
        /// User-defined custom variables.
        /// </summary>
        public Dictionary<string, List<string>> Custom { get; }

        public PassResultWithCustomHeaders(string path, string method, int statusCode, HttpHeaders customHeaders) :
            base(path, method, statusCode)
        {
            Custom = customHeaders.ToDictionary(x => x.Key, y => y.Value);
        }
    }
}
