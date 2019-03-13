using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.OpenApi.Results
{
    public class JsonResultSerializer : IResultSerializer
    {
        public string SerializeResult(List<Models.Result> results)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(JsonConvert.SerializeObject(results));

            return sb.ToString();
        }
    }
}
