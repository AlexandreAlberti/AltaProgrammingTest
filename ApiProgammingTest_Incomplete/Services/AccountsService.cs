using System.Collections.Generic;
using System;

namespace ApiProgrammingTest.Services
{
    public class AccountsService : IAccountsService
    {
        static readonly long MIN_TIME_TO_UPKEEP = 300_000; // Not do UpdateBalanceFromAccountUsingOwnedProperties more than once per 5 minute
        static readonly long MILLIS_IN_HOUR = 3_600_000; // 60min/h * 60sec/min * 1000ms/s
        readonly PropertyMogulContext context;

        public AccountsService(PropertyMogulContext context)
        {
            this.context = context;
        }

        private AccountInfo Transform(AccountInfoDB info)
        {
            if (info == null)
            {
                return new AccountInfo();
            }
            List<int> purchases = new List<int>();
            if (info.Purchases.Length > 0)
            {
                foreach (string propertyString in info.Purchases.Split(","))
                {
                    purchases.Add(Int32.Parse(propertyString));
                }
            }
            return new AccountInfo()
            {
                Id = info.Id,
                Name = info.Name,
                SignUpTime = info.SignUpTime,
                Balance = info.Balance,
                LastUpdateTime = info.LastUpdateTime,
                Purchases = purchases
            };

        }

        public IEnumerable<AccountInfo> Get()
        {
            List<AccountInfo> returnlist = new List<AccountInfo>();
            foreach (AccountInfoDB ac in context.Accounts)
            {
                UpdateBalanceFromAccountUsingOwnedProperties(ac.Id);
                returnlist.Add(Transform(ac));
            }

            return returnlist;
        }

        public AccountInfo Get(int id)
        {
            UpdateBalanceFromAccountUsingOwnedProperties(id);
            return Transform(context.Accounts.Find(id));
        }

        public void CreateAccount(string name)
        {
            System.DateTime now = System.DateTime.UtcNow;

            AccountInfoDB newAccount = new AccountInfoDB()
            {
                Name = name,
                SignUpTime = now,
                Balance = 5000,
                LastUpdateTime = now,
                Purchases = ""
            };

            context.Accounts.Add(newAccount);

            context.SaveChanges();
        }

        public bool UpdateAccountName(int id, string name)
        {
            AccountInfoDB existingAccount = context.Accounts.Find(id);

            if (existingAccount == null)
            {
                return false;
            }
            existingAccount.Name = name;
            context.Accounts.Update(existingAccount);
            context.SaveChanges();

            return true;
        }

        public bool UpdateAccountBalance(int id, decimal balance)
        {
            AccountInfoDB existingAccount = context.Accounts.Find(id);

            if (existingAccount == null)
            {
                return false;
            }

            existingAccount.Balance = balance;

            context.Accounts.Update(existingAccount);
            context.SaveChanges();

            return true;
        }

        public bool UpdateAccountProperties(int id, List<int> properties)
        {
            AccountInfoDB existingAccount = context.Accounts.Find(id);

            if (existingAccount == null)
            {
                return false;
            }

            string props = "";
            for (int i = 0; i < properties.Count; i++)
            {
                if (i > 0)
                {
                    props += ",";
                }
                props += properties[i];
            }

            existingAccount.Purchases = props;

            context.Accounts.Update(existingAccount);
            context.SaveChanges();

            return true;
        }
        public bool UpdateAccountBalanceAndProperties(int id, decimal balance, List<int> properties)
        {
            AccountInfoDB existingAccount = context.Accounts.Find(id);

            if (existingAccount == null)
            {
                return false;
            }

            string propertiesString = "";
            for (int i = 0; i < properties.Count; i++)
            {
                if (i > 0)
                {
                    propertiesString += ",";
                }
                propertiesString += properties[i];
            }

            existingAccount.Balance = balance;
            existingAccount.Purchases = propertiesString;

            context.Accounts.Update(existingAccount);
            context.SaveChanges();

            return true;
        }

        public bool DeleteAccount(int id)
        {
            UpdateBalanceFromAccountUsingOwnedProperties(id, true);
            AccountInfoDB existingAccount = context.Accounts.Find(id);

            if (existingAccount != null)
            {
                FreeProperties(Transform(existingAccount).Purchases);
                context.Accounts.Remove(existingAccount);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        private void FreeProperties(List<int> ids)
        {
            foreach (int id in ids)
            {
                PropertyInfo property = context.Properties.Find(id);

                if (property == null)
                {
                    continue;
                }

                property.AvailableForPurchase = true;
                property.OwnedBy = -1;
                context.Properties.Update(property);
            }
        }
        public decimal UpdateBalanceFromAccountUsingOwnedProperties(int id)
        {
            return UpdateBalanceFromAccountUsingOwnedProperties(id, false);
        }

        public decimal UpdateBalanceFromAccountUsingOwnedProperties(int id, bool forced)
        {
            AccountInfo accountToUpkeep = Transform(context.Accounts.Find(id));
            if (accountToUpkeep != null)
            {
                Double timePassedSinceLastUpdate = System.DateTime.UtcNow.Subtract(accountToUpkeep.LastUpdateTime).TotalMilliseconds;
                if ((timePassedSinceLastUpdate >= MIN_TIME_TO_UPKEEP || forced) && timePassedSinceLastUpdate >= 0)
                {
                    decimal hoursPassed = (decimal)timePassedSinceLastUpdate / MILLIS_IN_HOUR;
                    decimal newBalance = accountToUpkeep.Balance;
                    if (accountToUpkeep.Purchases != null)
                    {
                        foreach (int propId in accountToUpkeep.Purchases)
                        {
                            PropertyInfo property = context.Properties.Find(propId);

                            if (property == null)
                            {
                                continue;
                            }

                            newBalance += property.RevenuePerHour * hoursPassed;
                        }
                    }
                    UpdateAccountBalance(id, newBalance);
                    return newBalance;
                }
                return accountToUpkeep.Balance;
            }
            return decimal.Zero;
        }
    }
}
