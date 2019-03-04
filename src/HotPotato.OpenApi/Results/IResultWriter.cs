using HotPotato.Core.Models;

namespace HotPotato.OpenApi.Results
{
    public interface IResultWriter
    {
        void Write(HttpPair pair, Result result);
    }
}
