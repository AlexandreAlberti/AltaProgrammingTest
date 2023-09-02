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
        public IActionResult Purchase([FromBody] PurchaseInfo buyActionInfo)
        {
            if (!service.Buy(buyActionInfo))
            {
                // No property or not available for purchase
                return NoContent();
            }
            return Ok();
        }

        [HttpPost("sell")]
        public IActionResult Sell([FromBody] PurchaseInfo sellActionInfo)
        {
            if (!service.Sell(sellActionInfo))
            {
                // No property or not available for purchase
                return NoContent();
            }
            return Ok();
        }

        [HttpPost("transaction")]
        public IActionResult Transaction([FromBody] TransactionBetweenAccounts transactionBetweenAccounts)
        {
            if (!service.TransactonBetweenAccounts(transactionBetweenAccounts))
            {
                // No property or not available for purchase
                return NoContent();
            }
            return Ok();
        }

    }
}
