using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels.Transactions
{
    public class TransactionDetailsListViewModel
    {
        public int AccountId { get; set; }
        public IEnumerable<TransactionDetailsViewModel> Transactions { get; set; }
        public int TransactionsToSkipInList { get; set; } = 20;
    }

    public class TransactionDetailsViewModel
    {
        public int TransactionId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Symbol { get; set; }
        public string Bank { get; set; }
        public string Account { get; set; }
    }
}
