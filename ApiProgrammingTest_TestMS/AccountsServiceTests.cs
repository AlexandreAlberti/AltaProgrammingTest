using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiProgrammingTest;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace ApiProgrammingTest_TestMS
{
    [TestClass]
    public class AccountsServiceTests
    {

        [TestMethod]
        public void TestBasicUpkeep()
        {
            var now = System.DateTime.UtcNow;
            var mockSetAccounts = new Mock<DbSet<AccountInfoDB>>();
            mockSetAccounts.Setup(mock => mock.Find(1)).Returns(new AccountInfoDB()
            {
                Id = 1,
                Balance = 50_000,
                SignUpTime = now.AddHours(-1),
                LastUpdateTime = now.AddHours(-1), // 1 hour ago
                Name = "Test Name",
                Purchases = "1"
            });
            mockSetAccounts.Setup(mock => mock.Update(It.IsAny<AccountInfoDB>())).Callback(() => { }); // Do nothing
            var mockSetProperties = new Mock<DbSet<PropertyInfo>>();
            mockSetProperties.Setup(mock => mock.Find(1)).Returns(new PropertyInfo()
            {
                Id = 1,
                Name = "Test Property",
                OwnedBy = 1,
                AvailableForPurchase = false,
                BuyPrice = 1000,
                SellPrice = 1000,
                RevenuePerHour = 1000
            });
            var contextMock = new Mock<PropertyMogulContext>();
            contextMock.Object.Accounts = mockSetAccounts.Object;
            contextMock.Object.Properties = mockSetProperties.Object;
            contextMock.Setup(mock => mock.SaveChanges()).Callback(() => { }); // Do nothing
            ApiProgrammingTest.Services.IAccountsService service = new ApiProgrammingTest.Services.AccountsService(contextMock.Object);

            Assert.AreEqual(51_000, (int) service.UpdateBalanceFromAccountUsingOwnedProperties(1)); // Starting Balance + 1h complete from Revenue (no decimal wanted)
        }
    }
}
