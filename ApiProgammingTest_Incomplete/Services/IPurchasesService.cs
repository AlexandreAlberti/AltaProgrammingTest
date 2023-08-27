namespace ApiProgrammingTest.Services
{
    public interface IPurchasesService
    {
        public bool Buy(PurchaseInfo info);
        public bool Sell(PurchaseInfo info);
    }
}