using System.Collections.Generic;

namespace Bank.MoneyLaundererBatch.ReportObjects
{
    public class AccountReport
    {
        public int Id { get; init; }
        public IEnumerable<TransactionReport> Transactions { get; init; }
    }
}