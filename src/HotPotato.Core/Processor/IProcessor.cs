using HotPotato.Core.Models;

namespace HotPotato.Core.Processor
{
    public interface IProcessor
    {
        void Process(HttpPair pair);
    }
}
