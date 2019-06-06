using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotPotato.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public List<Item> Items { get; set; }
    }
}
