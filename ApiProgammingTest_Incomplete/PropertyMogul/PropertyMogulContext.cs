using Microsoft.EntityFrameworkCore;
using System;

namespace ApiProgrammingTest
{
    public class PropertyMogulContext : DbContext
    {
        public DbSet<AccountInfoDB> Accounts { get; set; }

        public DbSet<PropertyInfo> Properties { get; set; }

        public PropertyMogulContext(DbContextOptions<PropertyMogulContext> options) : base(options)
        {

        }

        public PropertyMogulContext()
        {

        }
    }

    public class PropertyInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal RevenuePerHour { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }
        public bool AvailableForPurchase { get; set; }
        public int OwnedBy { get; set; }
    }

    public class AccountInfoDB
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SignUpTime { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Purchases { get; set; }
    }

    //add anything extra to handle the users balance and property ownership
}
