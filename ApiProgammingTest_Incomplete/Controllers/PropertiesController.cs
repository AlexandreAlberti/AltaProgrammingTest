using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiProgrammingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {

        readonly Services.IPropertiesService service;
        readonly Services.IAccountsService serviceAccount;

        public PropertiesController(Services.IPropertiesService service, Services.IAccountsService serviceAccount)
        {
            this.service = service;
            this.serviceAccount = serviceAccount;
        }

        [HttpGet]
        public IEnumerable<PropertyInfo> Get()
        {
            return service.Get();
        }

        [HttpGet("{id}")]
        public IActionResult GetProperty(int id)
        {
            var property = service.Get(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpGet("/ownedBy/{id}")]
        public IActionResult GetPropertiesOwnedBy(int id)
        {
            var account = serviceAccount.Get(id);
            if (account == null)
            {
                return NotFound();
            }
            var properties = service.Get(account.Purchases);
            return Ok(properties);
        }

        [HttpPost]
        public PropertyInfo CreateProperty([FromBody] PropertyInfo property)
        {
            property.AvailableForPurchase = true;
            service.CreateProperty(property);
            return property;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProperty(int id, [FromBody] PropertyInfo property)
        {
            if (service.UpdateProperty(id, property.Name, property.BuyPrice, property.SellPrice, property.RevenuePerHour, property.AvailableForPurchase, property.OwnedBy))
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProperty(int id)
        {
            if (service.DeleteProperty(id))
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
