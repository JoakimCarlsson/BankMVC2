using System.Collections.Generic;

namespace Bank.MoneyLaundererBatch.ReportObjects
{
    public class CustomerReport
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public IEnumerable<AccountReport> Accounts { get; init; }
    }
}