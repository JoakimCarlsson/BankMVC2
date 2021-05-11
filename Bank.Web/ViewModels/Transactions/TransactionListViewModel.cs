using System.Collections.Generic;

namespace Bank.Web.ViewModels.Transactions
{
    public class TransactionListViewModel
    {
        public IEnumerable<TransactionDetailsViewModel> Transactions { get; set; }
    }
}
