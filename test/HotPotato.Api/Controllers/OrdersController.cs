using HotPotato.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
            ProblemDetails problemDetails = new ProblemDetails()
            {
                Type = "Bad Request",
                Status = 400,
                Title = "400 Bad Request",
                Detail = "Order needs an ID, Price, and Items[]"
            };

            if (OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == order.Id) != null)
            {
                problemDetails.Detail = $"Order with ID: {order.Id} already exists";
                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            if (order.Id == 0 || order.Price == 0 || order.Items == null)
            {
                

                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            OrderDataStore.Current.Orders.Add(order);

            var uri = $"https://localhost:44366/order/{order.Id}";

            return Created(uri, order);
        }

        [HttpGet("/order/{id}")]
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

        [HttpPut("/order/{id}")]
        public IActionResult PutOrderWithId([FromBody] Order oldOrder, int id)
        {
            var order = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id);

            if (order == null)
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

            OrderDataStore.Current.Orders[OrderDataStore.Current.Orders.FindIndex(r => r.Id == id)] = oldOrder;

            return NoContent();
        }

        [HttpGet("/order/{id}/price")]
        public IActionResult GetPriceWithId(int id)
        {
            var order = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id);

            if (order == null)
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

            var price = String.Format("{0:.00}", order.Price);

            return Ok(price);
        }

        [HttpGet("/order/{id}/items")]
        public IActionResult GetItemsForOrderId(int id)
        {
            var order = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id);

            if (order == null)
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

            return Ok(order.Items);
        }

        [HttpGet("/order/{id}/items/{itemId}")]
        public IActionResult GetItemWithIdFromOrderWithId(int id, int itemId)
        {
            var order = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id);
            var item = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id).Items.FirstOrDefault(r => r.ItemId == itemId);

            if (order == null || item == null)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Type = "Not Found",
                    Status = 404,
                    Title = "404 Not Found",
                    Detail = $"Order with ID: {id} was not found or Item with ID: {itemId} was not found"
                };

                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            var res = new ContentResult()
            {
                ContentType = "application/pdf",
                StatusCode = 200,
                Content = item.ToString()
            };

            return res;
        }

        [HttpDelete("/order/{id}/items/{itemId}")]
        public IActionResult DeleteItemWithItemIdInOrder(int id, int itemId)
        {
            var order = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id);
            var item = OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id).Items.FirstOrDefault(r => r.ItemId == itemId);

            if (order == null || item == null)
            {
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Type = "Not Found",
                    Status = 404,
                    Title = "404 Not Found",
                    Detail = $"Order with ID: {id} was not found or Item with ID: {itemId} was not found"
                };

                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            OrderDataStore.Current.Orders.FirstOrDefault(r => r.Id == id).Items.Remove(item);

            return NoContent();
        }
    }
}