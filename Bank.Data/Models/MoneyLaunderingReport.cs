using System;

namespace Bank.Data.Models
{
    public class MoneyLaunderingReport
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}