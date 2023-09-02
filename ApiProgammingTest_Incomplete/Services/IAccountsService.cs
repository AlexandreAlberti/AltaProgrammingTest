using System.Collections.Generic;

namespace ApiProgrammingTest.Services
{
    public interface IAccountsService
    {
        public IEnumerable<AccountInfo> Get();
        public AccountInfo Get(int id);
        public void CreateAccount(string name);
        public bool UpdateAccountName(int id, string name);
        public bool UpdateAccountBalance(int id, decimal balance);
        public bool UpdateAccountProperties(int id, List<int> properties);
        public bool UpdateAccountBalanceAndProperties(int id, decimal balance, List<int> properties);
        public bool DeleteAccount(int id);
        public decimal UpdateBalanceFromAccountUsingOwnedProperties(int id);
        public decimal UpdateBalanceFromAccountUsingOwnedProperties(int id, bool forced);
    }
}