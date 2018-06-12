using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payments.Application;

namespace Payments.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TestProvider1Controller : Controller
    {
        private readonly IPaymentsApplicationService _paymentsApplicationService;

        public TestProvider1Controller(IPaymentsApplicationService paymentsApplicationService)
        {
            _paymentsApplicationService = paymentsApplicationService;
        }

        public async Task<IActionResult> Cancel(Guid? orderId)
        {
            if (!orderId.HasValue)
                return BadRequest();

            await _paymentsApplicationService.CancelPaymentProcessAsync(orderId.Value);
            return Ok();
        }

        public async Task<IActionResult> Complete(Guid? orderId)
        {
            if (!orderId.HasValue)
                return BadRequest();

            await _paymentsApplicationService.CompletePaymentProcessAsync(orderId.Value);
            return Ok();
        }
    }
}