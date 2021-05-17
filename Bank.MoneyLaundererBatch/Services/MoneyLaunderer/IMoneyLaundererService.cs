using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.MoneyLaundererBatch.ReportObjects;

namespace Bank.MoneyLaundererBatch.Services.MoneyLaunderer
{
    public interface IMoneyLaundererService
    {
        public Task<IEnumerable<string>> GetCountries();
        public Task<List<CustomerReport>> GetTransactionsOverAmountAsync(DateTime date, string country, decimal amount);
        public Task<List<CustomerReport>> GetTransactionsOverAmountAndTimeAsync(DateTime date, string country, decimal amount, int hours);
        
    }
}