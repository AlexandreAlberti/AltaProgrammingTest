using System.Collections.Generic;

namespace ApiProgrammingTest.Services
{
    public interface IAccountsService
    {
        public IEnumerable<AccountInfo> Get();
        public AccountInfo Get(int id);
        public void CreateAccount(string name);
        public bool UpdateAccount(int id, string name, decimal balance, List<int> properties);
        public bool DeleteAccount(int id);
        public decimal Upkeep(int id);
        public decimal Upkeep(int id, bool forced);
    }
}