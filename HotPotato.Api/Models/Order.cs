using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotPotato.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public string[] Items { get; set; }
    }
}
