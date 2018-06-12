using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payments.Application;

namespace Payments.WebApi.Controllers
{
    public class AddProductToOrderModel
    {
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }

    [Route("api/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IOrdersApplicationService _ordersApplicationService;

        public OrderController(IOrdersApplicationService ordersApplicationService)
        {
            _ordersApplicationService = ordersApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest();

            var orderId = await _ordersApplicationService.CreateOrderAsync(username);
            return Ok(orderId);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]AddProductToOrderModel request)
        {
            await _ordersApplicationService.AddProductAsync(request.OrderId, request.Name, request.Count, request.Price);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> BeginPayment(Guid orderId)
        {
            await _ordersApplicationService.ProcessToPaymentAsync(orderId);
            return Ok();
        }
    }
}