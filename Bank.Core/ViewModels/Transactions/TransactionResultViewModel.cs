using Bank.Core.Enums;

namespace Bank.Core.ViewModels.Transactions
{
    public class TransactionResultViewModel
    {
        public int TransactionId { get; set; }
        public TransactionResultCode Result { get; set; }
    }
}
