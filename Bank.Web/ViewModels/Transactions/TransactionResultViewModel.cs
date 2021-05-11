using Bank.Web.Enums;

namespace Bank.Web.ViewModels.Transactions
{
    public class TransactionResultViewModel
    {
        public int TransactionId { get; set; }
        public TransactionResultCode Result { get; set; }
    }
}
