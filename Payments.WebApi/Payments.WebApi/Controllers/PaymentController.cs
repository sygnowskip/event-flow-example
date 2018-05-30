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

    public class PaymentController : Controller
    {
        private readonly IPaymentsApplicationService _paymentsApplicationService;

        public PaymentController(IPaymentsApplicationService paymentsApplicationService)
        {
            _paymentsApplicationService = paymentsApplicationService;
        }

        // GET
        public async Task<IActionResult> Begin(BeginPaymentModel request)
        {
            var redirectUrl = await _paymentsApplicationService.BeginPaymentProcessAsync(request.Country,
                request.Currency, request.System, request.ExternalId, request.ExternalCallbackUrl, request.Amount);
            return Ok(redirectUrl);
        }
    }
}