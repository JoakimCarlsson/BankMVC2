using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.MoneyLaundererBatch.ReportObjects;

namespace Bank.MoneyLaundererBatch.Services.Email
{
    class EmailService : IEmailService
    {
        public async Task SendReportEmailAsync(string country, IEnumerable<CustomerReport> reports)
        {
            
        }
    }
}