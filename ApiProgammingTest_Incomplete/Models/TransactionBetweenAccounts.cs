namespace ApiProgrammingTest
{
    public class TransactionBetweenAccounts
    {
        public int IdProperty { get; set; }
        public int IdAccountFrom { get; set; }
        public int IdAccountTo { get; set; }
        public bool TransactionUsedOwnPrice { get; set; }
        public decimal PurchasePriceAgreement { get; set; }
    }
}