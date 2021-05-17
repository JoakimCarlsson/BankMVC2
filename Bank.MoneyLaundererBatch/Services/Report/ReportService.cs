using System;
using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Models;
using Bank.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Bank.MoneyLaundererBatch.Services.Report
{
    class ReportService : IReportService
    {
        private readonly IAsyncRepository<MoneyLaunderingReport> _moneyLaunderingRepository;

        public ReportService(IAsyncRepository<MoneyLaunderingReport> moneyLaunderingRepository)
        {
            _moneyLaunderingRepository = moneyLaunderingRepository;
        }

        public async Task SaveReportAsync(MoneyLaunderingReport report)
        {
            report.EndDate = DateTime.Now;
            await _moneyLaunderingRepository.AddAsync(report);
        }

        public async Task<DateTime> GetLastRanTimeAsync()
        {
            var reports = await _moneyLaunderingRepository.ListAllAsync();

            if (!reports.Any())
                return DateTime.Now;

            var lastReport = await reports.FirstAsync();
            return lastReport.EndDate;
        }
    }
}