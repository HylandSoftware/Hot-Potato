using HotPotato.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Storage
{
    public interface IHttpPairStorage
    {
        Task Store(HttpPair pair);
        Task<HttpPair> Retrieve(Guid id);
    }
}
