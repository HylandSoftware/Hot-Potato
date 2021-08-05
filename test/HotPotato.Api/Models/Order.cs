using System.Collections.Generic;

namespace HotPotato.Test.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double? Price { get; set; }
        public List<Item> Items { get; set; }
    }
}
