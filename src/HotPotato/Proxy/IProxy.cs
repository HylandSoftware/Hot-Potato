using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotPotato.Proxy
{
    public interface IProxy
    {
        Task ProcessAsync(string remoteEndpoint, HttpRequest requestIn, HttpResponse responseOut);
    }
}
