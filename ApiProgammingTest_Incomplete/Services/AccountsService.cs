using System.Collections.Generic;
using System;

namespace ApiProgrammingTest.Services
{
    public class AccountsService : IAccountsService
    {
        static readonly long MIN_TIME_TO_UPKEEP = 300_000; // Not do Upkeep more than once per 5 minute
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
                foreach (string s in info.Purchases.Split(","))
                {
                    purchases.Add(Int32.Parse(s));
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
                Upkeep(ac.Id);
                returnlist.Add(Transform(ac));
            }

            return returnlist;
        }

        public AccountInfo Get(int id)
        {
            Upkeep(id);
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

        public bool UpdateAccount(int id, string name, decimal balance, List<int> properties)
        {
            var existing = context.Accounts.Find(id);

            if (existing == null)
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

            existing.Name = name;
            existing.Balance = balance;
            existing.Purchases = props;

            context.Accounts.Update(existing);
            context.SaveChanges();

            return true;
        }

        public bool DeleteAccount(int id)
        {
            Upkeep(id, true);
            var existing = context.Accounts.Find(id);

            if (existing != null)
            {
                FreeProperties(Transform(existing).Purchases);
                context.Accounts.Remove(existing);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        private void FreeProperties(List<int> ids)
        {
            foreach (int id in ids)
            {
                var existing = context.Properties.Find(id);

                if (existing == null)
                {
                    continue;
                }

                existing.AvailableForPurchase = true;
                existing.OwnedBy = -1;
                context.Properties.Update(existing);
            }
        }
        public decimal Upkeep(int id)
        {
            return Upkeep(id, false);
        }

        public decimal Upkeep(int id, bool forced)
        {
            var accountToUpkeep = Transform(context.Accounts.Find(id));
            if (accountToUpkeep != null)
            {
                var timePassed = System.DateTime.UtcNow.Subtract(accountToUpkeep.LastUpdateTime).TotalMilliseconds;
                if ((timePassed >= MIN_TIME_TO_UPKEEP || forced) && timePassed >= 0)
                {
                    decimal hoursPassed = (decimal)timePassed / MILLIS_IN_HOUR;
                    decimal balance = accountToUpkeep.Balance;
                    if (accountToUpkeep.Purchases != null)
                    {
                        foreach (int propId in accountToUpkeep.Purchases)
                        {
                            var existing = context.Properties.Find(propId);

                            if (existing == null)
                            {
                                continue;
                            }

                            balance += existing.RevenuePerHour * hoursPassed;
                        }
                    }
                    UpdateAccount(id, accountToUpkeep.Name, balance, accountToUpkeep.Purchases);
                    return balance;
                }
                return accountToUpkeep.Balance;
            }
            return decimal.Zero;
        }
    }
}
