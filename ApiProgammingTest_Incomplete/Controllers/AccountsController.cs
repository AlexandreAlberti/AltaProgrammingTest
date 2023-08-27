using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiProgrammingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly Services.IAccountsService service;

        public AccountsController(Services.IAccountsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<AccountInfo> Get()
        {
            return service.Get();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = service.Get(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public void CreateAccount([FromBody] AccountInfo request)
        {
            service.CreateAccount(request.Name);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] AccountInfo account)
        {
            var accountInfo = service.Get(id);

            if (accountInfo == null)
            {
                return NotFound();
            }
            if (service.UpdateAccount(id, account.Name, service.Upkeep(id), accountInfo.Purchases))
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            if (service.DeleteAccount(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
