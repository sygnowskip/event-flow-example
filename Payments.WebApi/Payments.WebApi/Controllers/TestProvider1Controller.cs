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

        public async Task<IActionResult> Cancel(string externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return BadRequest();

            await _paymentsApplicationService.CancelPaymentProcessAsync(externalId);
            return Ok();
        }
    }
}