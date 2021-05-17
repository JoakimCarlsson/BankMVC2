using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.MoneyLaundererBatch.ReportObjects;

namespace Bank.MoneyLaundererBatch.Services.Email
{
    public interface IEmailService
    {
        public Task SendReportEmailAsync(string country, IEnumerable<CustomerReport> reports);
    }
}