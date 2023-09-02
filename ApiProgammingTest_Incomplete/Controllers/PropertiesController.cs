using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiProgrammingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {

        readonly Services.IPropertiesService serviceProperties;
        readonly Services.IAccountsService serviceAccount;

        public PropertiesController(Services.IPropertiesService service, Services.IAccountsService serviceAccount)
        {
            this.serviceProperties = service;
            this.serviceAccount = serviceAccount;
        }

        [HttpGet]
        public IEnumerable<PropertyInfo> Get()
        {
            return serviceProperties.Get();
        }

        [HttpGet("{id}")]
        public IActionResult GetProperty(int id)
        {
            PropertyInfo property = serviceProperties.Get(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpGet("/ownedBy/{id}")]
        public IActionResult GetPropertiesOwnedBy(int id)
        {
            AccountInfo account = serviceAccount.Get(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(serviceProperties.Get(account.Purchases));
        }

        [HttpPost]
        public PropertyInfo CreateProperty([FromBody] PropertyInfo property)
        {
            property.AvailableForPurchase = true;
            serviceProperties.CreateProperty(property);
            return property;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProperty(int id, [FromBody] PropertyInfo property)
        {
            var existingProperty = serviceProperties.Get(id);
            if (existingProperty == null)
            {
                return NotFound();
            }

            ISet<PropertyUpdatePropietyEnum> propietiesListToUpdate = new HashSet<PropertyUpdatePropietyEnum>();

            if (!existingProperty.Name.Equals(property.Name))
            {
                propietiesListToUpdate.Add(PropertyUpdatePropietyEnum.NAME);
            }
            if (!existingProperty.SellPrice.Equals(property.SellPrice))
            {
                propietiesListToUpdate.Add(PropertyUpdatePropietyEnum.SELL_PRICE);
            }
            if (!existingProperty.BuyPrice.Equals(property.BuyPrice))
            {
                propietiesListToUpdate.Add(PropertyUpdatePropietyEnum.BUY_PRICE);
            }
            if (!existingProperty.RevenuePerHour.Equals(property.RevenuePerHour))
            {
                propietiesListToUpdate.Add(PropertyUpdatePropietyEnum.REVENUE_PER_HOUR);
            }
            if (!serviceProperties.UpdateProperty(id, property, propietiesListToUpdate))
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProperty(int id)
        {
            if (serviceProperties.DeleteProperty(id))
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
