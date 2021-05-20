namespace Bank.Web.ViewModels.Transactions
{
    public class TransferViewModel : TransactionBaseViewModel
    {
        public int ToAccountId { get; set; }
        public bool ToAnotherBank { get; set; }
    }
}
