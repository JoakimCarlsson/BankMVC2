using System;

namespace Bank.MoneyLaundererBatch.ReportObjects
{
    public class TransactionReport
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
        public decimal Amount { get; init; }
    }
}