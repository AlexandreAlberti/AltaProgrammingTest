using System.Collections.Generic;
using System;

namespace ApiProgrammingTest.Services
{
    public class PurchasesService : IPurchasesService
    {
        Services.IAccountsService serviceAcc;
        Services.IPropertiesService serviceProp;

        public PurchasesService(Services.IPropertiesService serviceProp, Services.IAccountsService serviceAcc)
        {
            this.serviceProp = serviceProp;
            this.serviceAcc = serviceAcc;
        }

        bool IPurchasesService.Buy(PurchaseInfo info)
        {
            serviceAcc.Upkeep(info.IdAccount, true);
            var property = serviceProp.Get(info.IdProperty);
            if (property == null || !property.AvailableForPurchase)
            {
                // No property or not available for purchase
                return false;
            }

            var account = serviceAcc.Get(info.IdAccount);
            if (account == null || account.Balance < property.BuyPrice)
            {
                // No account or not enough balance
                return false;
            }

            var existingPurchase = account.Purchases.Contains(info.IdProperty);
            if (existingPurchase)
            {
                // AlreadyOwning
                return false;
            }
            decimal newBalance = account.Balance - property.BuyPrice;
            List<int> purchases = account.Purchases;
            purchases.Add(info.IdProperty);
            serviceAcc.UpdateAccount(account.Id, account.Name, newBalance, purchases);
            serviceProp.UpdateProperty(property.Id, property.Name, property.BuyPrice, property.SellPrice, property.RevenuePerHour, false, account.Id);
            return true;
        }

        bool IPurchasesService.Sell(PurchaseInfo info)
        {
            serviceAcc.Upkeep(info.IdAccount, true);
            var property = serviceProp.Get(info.IdProperty);
            if (property == null || property.AvailableForPurchase)
            {
                return false;
            }

            var account = serviceAcc.Get(info.IdAccount);
            if (account == null)
            {
                return false;
            }

            var existingPurchase = account.Purchases.Contains(info.IdProperty);
            if (!existingPurchase)
            {
                // Can't sell what you don't own
                return false;
            }
            decimal newBalance = account.Balance + property.SellPrice;
            List<int> purchases = account.Purchases;
            purchases.Remove(info.IdProperty);
            serviceAcc.UpdateAccount(account.Id, account.Name, newBalance, purchases);
            serviceProp.UpdateProperty(property.Id, property.Name, property.BuyPrice, property.SellPrice, property.RevenuePerHour, true, -1);
            return true;
        }
    }
}