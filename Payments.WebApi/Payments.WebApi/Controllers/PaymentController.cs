using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payments.Application;

namespace Payments.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentsApplicationService _paymentsApplicationService;

        public PaymentController(IPaymentsApplicationService paymentsApplicationService)
        {
            _paymentsApplicationService = paymentsApplicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Ping(Guid? orderId)
        {
            if (!orderId.HasValue)
                return BadRequest();

            await _paymentsApplicationService.PingAsync(orderId.Value);
            return Ok();
        }
    }
}