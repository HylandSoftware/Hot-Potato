using HotPotato.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;

namespace HotPotato.Api.Controllers
{
    public class OrdersController : Controller
    {
        #region HAPPY PATHS

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
            //Invalid Body condition -- Missing properties
            if (id == 555)
            {
                var t = new { Price = 20.50 };

                return Ok(t);
            }

            //Missing Body -- return Empty Body
            if(id == 777)
            {
                return Ok();
            }

            //MissingContent -- return content-type not listed
            if(id == 888)
            {
                return Ok("Wrong Content-Type");
            }

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

        [HttpOptions("/order/{id}")]
        public IActionResult OptionsForPath()
        {
            Response.Headers.Add("Allow", "Allow");
            return Ok("GET, PUT");
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
            //UnexpectedBody -- return unexpected body on 204 No Content
            if(id == 555 && itemId == 555)
            {
                var res = new ContentResult()
                {
                    ContentType = "text/plain",
                    Content = "Unexpected Body",
                };
                return res;
            }

            //InValidHeaders -- strings not formatted correctly
            if (id == 666 && itemId == 666)
            {
                Response.Headers.Add("X-header", "&*(&^&%%##@");
                return NoContent();
            }

            //MissingHeaders -- Omit Header defined in spec
            if(id == 777 && itemId == 777)
            {
                Response.Headers.Clear();
                return NoContent();    
            }

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

            Response.Headers.Add("X-header", "HEADER");

            return NoContent();
        }

        #endregion

        #region NOT IN SPEC PATHS

        /// <summary>
        /// PATH NOT IN SPEC! HotPotato should catch this call and return an error.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/missingpath")]
        public IActionResult GetMissingPath()
        {
            var path = "This PATH is missing in the spec!";
            return Ok(path);
        }

        /// <summary>
        /// METHOD NOT IN SPEC! HotPotato should catch this call and return an error.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/missingmethod")]
        public IActionResult GetMissingMethod()
        {
            var method = "This METHOD is missing in the spec!";
            return Ok(method);
        }

        /// <summary>
        /// STATUS CODE NOT IN SPEC! HotPotato should catch this call and return an error.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/missingcode")]
        public IActionResult GetMissingStatusCode()
        {
            var code = "This STATUS CODE is missing in the spec!";
            return Ok(code);
        }

        #endregion
    }
}