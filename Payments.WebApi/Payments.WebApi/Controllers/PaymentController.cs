using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Payments;

namespace Payments.WebApi.Controllers
{
    public class BeginPaymentModel
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public string System { get; set; }
        public string ExternalId { get; set; }
        public string ExternalCallbackUrl { get; set; }
        public decimal Amount { get; set; }
    }

    [Route("api/[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentsApplicationService _paymentsApplicationService;

        public PaymentController(IPaymentsApplicationService paymentsApplicationService)
        {
            _paymentsApplicationService = paymentsApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Begin([FromBody]BeginPaymentModel request)
        {
            var redirectUrl = await _paymentsApplicationService.BeginPaymentProcessAsync(request.Country,
                request.Currency, request.System, request.ExternalId, request.ExternalCallbackUrl, request.Amount);
            return Ok(redirectUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Ping(string externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return BadRequest();

            await _paymentsApplicationService.PingAsync(externalId);
            return Ok();
        }
    }
}