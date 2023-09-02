using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiProgrammingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly Services.IAccountsService accountService;

        public AccountsController(Services.IAccountsService service)
        {
            this.accountService = service;
        }

        [HttpGet]
        public IEnumerable<AccountInfo> Get()
        {
            return accountService.Get();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            AccountInfo account = accountService.Get(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public void CreateAccount([FromBody] AccountInfo request)
        {
            accountService.CreateAccount(request.Name);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] AccountInfo account)
        {
            AccountInfo accountInfo = accountService.Get(id);

            if (accountInfo == null)
            {
                return NotFound();
            }
            accountService.UpdateBalanceFromAccountUsingOwnedProperties(id);
            if (accountService.UpdateAccountName(id, account.Name))
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            if (accountService.DeleteAccount(id))
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
