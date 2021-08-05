using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotPotato.Test.Api.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
    }
}
