using System;
using Bank.Core.Enums;

namespace Bank.Core.ViewModels.Transactions
{
    public class TransactionConfirmationViewModel
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Symbol { get; set; }
    }
}
