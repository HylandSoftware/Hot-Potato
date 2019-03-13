using System.Collections.Generic;

namespace HotPotato.OpenApi.Results
{
    public interface IResultSerializer
    {
        string SerializeResult(List<Models.Result> results);
    }
}
