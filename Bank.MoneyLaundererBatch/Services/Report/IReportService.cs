using System;
using System.Threading.Tasks;
using Bank.Data.Models;

namespace Bank.MoneyLaundererBatch.Services.Report
{
    public interface IReportService
    {
        public Task SaveReportAsync(MoneyLaunderingReport report);
        public Task<DateTime> GetLastRanTimeAsync();
    }
}