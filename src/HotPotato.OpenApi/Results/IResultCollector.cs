using HotPotato.Core.Models;

namespace HotPotato.OpenApi.Results
{
    public interface IResultCollector
    {
        void WriteResults(HttpPair pair, Result result);
    }
}
