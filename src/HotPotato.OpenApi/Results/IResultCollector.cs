using HotPotato.Core.Models;

namespace HotPotato.OpenApi.Results
{
    public interface IResultCollector
    {
        void Add(HttpPair pair, Result result);
    }
}
