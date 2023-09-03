using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ApiProgrammingTest
{
    public class PropertyMogulContext : DbContext
    {
        public DbSet<AccountInfoDB> Accounts { get; set; }
        public DbSet<AccountName> AccountNames { get; set; }

        public DbSet<PropertyInfo> Properties { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }

        public PropertyMogulContext(DbContextOptions<PropertyMogulContext> options) : base(options)
        {

        }

        public PropertyMogulContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountInfoDB>()
                .HasIndex(account => account.Name)
                .IsUnique();
        }

    }

    public class PropertyInfo
    {
        [Key]
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
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SignUpTime { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Purchases { get; set; }
    }
    public class AccountName
    {
        [Key]
        public string Name { get; set; }
    }
    public class TransactionLog
    {
        [Key]
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public DateTime OperationTimestamp { get; set; }
    }

    //add anything extra to handle the users balance and property ownership
}
