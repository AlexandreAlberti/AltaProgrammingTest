using System.Collections.Generic;
using System;

namespace ApiProgrammingTest.Services
{
    public class PurchasesService : IPurchasesService
    {
        Services.IAccountsService serviceAccounts;
        Services.IPropertiesService serviceProperties;

        public PurchasesService(Services.IPropertiesService serviceProp, Services.IAccountsService serviceAcc)
        {
            this.serviceProperties = serviceProp;
            this.serviceAccounts = serviceAcc;
        }

        bool IPurchasesService.Buy(PurchaseInfo info)
        {
            serviceAccounts.UpdateBalanceFromAccountUsingOwnedProperties(info.IdAccount, true);
            PropertyInfo property = serviceProperties.Get(info.IdProperty);
            if (property == null || !property.AvailableForPurchase)
            {
                // No property or not available for purchase
                return false;
            }

            AccountInfo account = serviceAccounts.Get(info.IdAccount);
            if (account == null || account.Balance < property.BuyPrice)
            {
                // No account or not enough balance
                return false;
            }

            bool existingPurchase = account.Purchases.Contains(info.IdProperty);
            if (existingPurchase)
            {
                // AlreadyOwning
                return false;
            }
            decimal newBalance = account.Balance - property.BuyPrice;
            List<int> purchases = account.Purchases;
            purchases.Add(info.IdProperty);
            serviceAccounts.UpdateAccountBalanceAndProperties(account.Id, newBalance, purchases);
            serviceProperties.UpdatePropertyOwnership(property.Id, false, account.Id);

            return true;
        }

        bool IPurchasesService.Sell(PurchaseInfo info)
        {
            serviceAccounts.UpdateBalanceFromAccountUsingOwnedProperties(info.IdAccount, true);
            PropertyInfo property = serviceProperties.Get(info.IdProperty);
            if (property == null || property.AvailableForPurchase)
            {
                return false;
            }

            AccountInfo account = serviceAccounts.Get(info.IdAccount);
            if (account == null)
            {
                return false;
            }

            bool existingPurchase = account.Purchases.Contains(info.IdProperty);
            if (!existingPurchase)
            {
                // Can't sell what you don't own
                return false;
            }
            decimal newBalance = account.Balance + property.SellPrice;
            List<int> purchases = account.Purchases;
            purchases.Remove(info.IdProperty);
            serviceAccounts.UpdateAccountBalanceAndProperties(account.Id, newBalance, purchases);
            serviceProperties.UpdatePropertyOwnership(property.Id, true, -1);
            return true;
        }

        public bool TransactonBetweenAccounts(TransactionBetweenAccounts transactionBetweenAccounts)
        {
            PropertyInfo property = serviceProperties.Get(transactionBetweenAccounts.IdProperty);
            if (property == null || !property.AvailableForPurchase)
            {
                // No property or property has no owner
                return false;
            }

            AccountInfo accountFrom = serviceAccounts.Get(transactionBetweenAccounts.IdAccountFrom);
            if (accountFrom == null)
            {
                // No account
                return false;
            }
            bool ownerOfPropertyToSell = accountFrom.Purchases.Contains(transactionBetweenAccounts.IdProperty);
            if (!ownerOfPropertyToSell)
            {
                // From Account does not own the property
                return false;
            }
            // Maybe the accounts agreed on something outside the platform and we may keep track of this to know proper user Balances.
            decimal propertyBuyPrice = transactionBetweenAccounts.TransactionUsedOwnPrice ? transactionBetweenAccounts.PurchasePriceAgreement : property.BuyPrice;
            decimal propertySellPrice = transactionBetweenAccounts.TransactionUsedOwnPrice ? transactionBetweenAccounts.PurchasePriceAgreement : property.SellPrice;

            AccountInfo accountTo = serviceAccounts.Get(transactionBetweenAccounts.IdAccountTo);
            if (accountTo == null || accountTo.Balance < propertyBuyPrice)
            {
                // No account or not enough balance
                return false;
            }

            decimal newBalanceForBuyer = accountTo.Balance - propertyBuyPrice;
            List<int> purchasesBuyer = accountTo.Purchases;
            purchasesBuyer.Add(property.Id);
            serviceAccounts.UpdateAccountBalanceAndProperties(accountTo.Id, newBalanceForBuyer, purchasesBuyer);

            decimal newBalanceForSeller = accountTo.Balance + propertySellPrice;
            List<int> purchasesFromSeller = accountTo.Purchases;
            purchasesFromSeller.Add(property.Id);
            serviceAccounts.UpdateAccountBalanceAndProperties(accountTo.Id, newBalanceForBuyer, purchasesFromSeller);
            serviceProperties.UpdatePropertyOwnership(property.Id, false, accountTo.Id);
            return true;
        }
    }
}