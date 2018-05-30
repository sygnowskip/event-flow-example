using Microsoft.AspNetCore.Mvc;

namespace Payments.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "API is working";
        }
    }
}