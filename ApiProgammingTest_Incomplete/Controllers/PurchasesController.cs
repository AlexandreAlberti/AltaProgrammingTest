using Microsoft.AspNetCore.Mvc;

namespace ApiProgrammingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        readonly Services.IPurchasesService service;

        public PurchasesController(Services.IPurchasesService service)
        {
            this.service = service;
        }

        [HttpPost("buy")]
        public IActionResult Purchase([FromBody] PurchaseInfo purchase)
        {
            if (!service.Buy(purchase))
            {
                // No property or not available for purchase
                return NoContent();
            }
            return Ok();
        }

        [HttpPost("sell")]
        public IActionResult Sell([FromBody] PurchaseInfo purchase)
        {
            if (!service.Sell(purchase))
            {
                // No property or not available for purchase
                return NoContent();
            }
            return Ok();
        }

    }
}
