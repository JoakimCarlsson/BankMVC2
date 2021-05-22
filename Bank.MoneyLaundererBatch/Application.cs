using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Models;
using Bank.MoneyLaundererBatch.ReportObjects;
using Bank.MoneyLaundererBatch.Services;
using Bank.MoneyLaundererBatch.Services.Email;
using Bank.MoneyLaundererBatch.Services.MoneyLaunderer;
using Bank.MoneyLaundererBatch.Services.Report;

namespace Bank.MoneyLaundererBatch
{
    public class Application
    {
        private readonly IMoneyLaundererService _moneyLaundererService;
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;

        public Application(IMoneyLaundererService moneyLaundererService, IReportService reportService, IEmailService emailService)
        {
            _moneyLaundererService = moneyLaundererService;
            _reportService = reportService;
            _emailService = emailService;
        }
        
        public async Task RunAsync()
        {
            var moneyLaunderingReport = new MoneyLaunderingReport {StartDate = DateTime.Now};
            var lastReportDate = await _reportService.GetLastRanTimeAsync();
            var checkDate = lastReportDate.AddDays(1);

            var countries = await _moneyLaundererService.GetCountries();
            var reports = new List<CustomerReport>();
            foreach (string country in countries)
            {
                reports.AddRange(await _moneyLaundererService.GetTransactionsOverAmountAsync(checkDate, country, 15000));
                reports.AddRange(await _moneyLaundererService.GetTransactionsOverAmountAndTimeAsync(checkDate, country, 23000, 72));
                reports.AddRange(await _moneyLaundererService.GetTransactionsLessThanAmountAsync(checkDate, country, 15000));
                reports.AddRange(await _moneyLaundererService.GetTransactionsLessThanAmountAndTimeAsync(checkDate, country, 23000, 72));
                
                if (reports.Any())
                {
                    await _emailService.SendReportEmailAsync(country, reports);
                }
                reports.Clear();
            }

            await _reportService.SaveReportAsync(moneyLaunderingReport);
        }
    }
}