using System.Collections.Generic;

namespace Bank.API.ViewModels.Transactions
{
    public class TransactionDetailsListViewModel
    {
        public IEnumerable<TransactionDetailViewModel> Transactions { get; set; }
    }
}