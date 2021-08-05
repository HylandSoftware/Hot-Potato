using HotPotato.Test.Api.Models;
using System.Collections.Generic;

namespace HotPotato.Test.Api
{
    public class OrderDataStore
    {
        public static OrderDataStore Current { get; set; } = new OrderDataStore();
        public List<Order> Orders { get; set; }

        public OrderDataStore()
        {
            Orders = new List<Order>()
            {
                new Order()
                {
                    Id = 1,
                    Price = 30.00,
                    Items = new List<Item>()
                    {
                        new Item()
                        {
                            ItemId = 1,
                            Name = "Paper",
                            Price = 10.00
                        },
                        new Item()
                        {
                            ItemId = 2,
                            Name = "Pencils",
                            Price = 10.00
                        },new Item()
                        {
                            ItemId = 3,
                            Name = "Pens",
                            Price = 10.00
                        }
                    }
                },
                new Order()
                {
                    Id = 2,
                    Price = 15.00,
                    Items = new List<Item>()
                    {
                        new Item()
                        {
                            ItemId = 4,
                            Name = "Post-Its",
                            Price = 5.00
                        },
                        new Item()
                        {
                            ItemId = 5,
                            Name = "Markers",
                            Price = 10.00
                        }
                    }
                },
                new Order()
                {
                    Id = 3,
                    Price = 5.00,
                    Items = new List<Item>()
                    {
                        new Item()
                        {
                            ItemId = 4,
                            Name = "Post-Its",
                            Price = 5.00
                        }
                    }
                },
                new Order()
                {
                    Id = 4,
                    Price = null,
                    Items = new List<Item>()
                    {
                        new Item()
                        {
                            ItemId = 6,
                            Name = "Video Card",
                            Price = null
                        }
                    }
                }
            };
        }
    }
}
