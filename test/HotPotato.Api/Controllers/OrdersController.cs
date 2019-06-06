using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HotPotato.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotPotato.Api.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet("/")]
        public IActionResult getLandingPage()
        {
            string repo = @"https://bitbucket.hylandqa.net/projects/AUTOTEST/repos/hot-potato/browse";

            return Ok(repo);
        }

        [HttpGet("/order")]
        public IActionResult GetOrders()
        {
            return Ok(OrderDataStore.Current.Orders);
        }

        //What if an Order with the ID given already exists???
        //How are we going to continually test this?
        [HttpPost("/order")]
        public IActionResult PostOrder([FromBody] Order order)
        {
            if (order.Id == 0 || order.Price == 0 || order.Items == null)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Type = "Bad Request",
                    Status = 400,
                    Title = "400 Bad Request",
                    Detail = "Order needs an ID, Price, and Items[]"
                };

                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            //OrderDataStore.Current.Orders.Add(order);
            var uri = $"https://localhost:44366/order/{order.Id}";

            return Created(uri, order);
        }

        [HttpGet]
        public IActionResult GetOrderWithId(int id)
        {
            var order = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id);

            if(order == null)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Type = "Not Found",
                    Status = 404,
                    Title = "404 Not Found",
                    Detail = $"Order with ID: {id} was not found"
                };

                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            return Ok(order);
        }
    }
}