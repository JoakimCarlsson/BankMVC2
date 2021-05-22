using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Repositories.Customer;
using Bank.MoneyLaundererBatch.ReportObjects;
using Microsoft.EntityFrameworkCore;

namespace Bank.MoneyLaundererBatch.Services.MoneyLaunderer
{
    class MoneyLaundererService : IMoneyLaundererService
    {
        private readonly ICustomerRepository _customerRepository;

        public MoneyLaundererService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<string>> GetCountries() => _customerRepository.ListAllAsync().GetAwaiter().GetResult().Select(i => i.Country).Distinct();

        public async Task<List<CustomerReport>> GetTransactionsOverAmountAsync(DateTime date, string country, decimal amount)
        {
            var customers = await _customerRepository.ListAllAsync();
            return await customers
                .Where(c => c.Country == country && c.Dispositions.Any(d => d.Type == "OWNER" && d.Account.Transactions.Any(t => t.Amount > amount && t.Date == date)))
                .Select(c => new CustomerReport
                {
                    Id = c.CustomerId,
                    Name = $"{c.Givenname} {c.Surname}",
                    Accounts = c.Dispositions.Select(d => new AccountReport
                    {
                        Id = d.AccountId,
                        Transactions = d.Account.Transactions.Where(t => t.Amount > amount && t.Date == date)
                            .Select(t => new TransactionReport
                            {
                                Id = t.TransactionId,
                                Date = t.Date,
                                Amount = t.Amount
                            })
                    })
                }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CustomerReport>> GetTransactionsLessThanAmountAsync(DateTime date, string country, decimal amount)
        {
            var customers = await _customerRepository.ListAllAsync();
            return await customers
                .Where(c => c.Country == country && c.Dispositions.Any(d => d.Type == "OWNER" && d.Account.Transactions.Any(t => -t.Amount > amount && t.Date == date)))
                .Select(c => new CustomerReport
                {
                    Id = c.CustomerId,
                    Name = $"{c.Givenname} {c.Surname}",
                    Accounts = c.Dispositions.Select(d => new AccountReport
                    {
                        Id = d.AccountId,
                        Transactions = d.Account.Transactions.Where(t => -t.Amount > amount && t.Date == date)
                            .Select(t => new TransactionReport
                            {
                                Id = t.TransactionId,
                                Date = t.Date,
                                Amount = t.Amount
                            })
                    })
                }).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CustomerReport>> GetTransactionsOverAmountAndTimeAsync(DateTime date, string country, decimal amount, int hours)
        {
            var time = date.AddHours(hours * -1);
            
            var customers = await _customerRepository.ListAllAsync();
            
            return await customers.Where(c => c.Country == country && c.Dispositions.Any(d => d.Account.Transactions
                .Where(t => t.Date <= date && t.Date >= time)
                .Sum(t => t.Amount) > amount))
                .Select(c => new CustomerReport
                {
                    Id = c.CustomerId,
                    Name = $"{c.Givenname} {c.Surname}",
                    Accounts = c.Dispositions
                        .Where(a => a.Account.Transactions
                            .Where(t => t.Date <= date && t.Date >= time)
                            .Sum(t => t.Amount) > amount)
                        .Select(d => new AccountReport
                        {
                            Id = d.Account.AccountId,
                            Transactions = d.Account.Transactions
                                .Where(t => t.Date <= date && t.Date >= time)
                                .Select(t => new TransactionReport()
                                {
                                    Id = t.TransactionId,
                                    Date = t.Date,
                                    Amount = t.Amount
                                })
                        })
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<CustomerReport>> GetTransactionsLessThanAmountAndTimeAsync(DateTime date, string country, decimal amount, int hours)
        {
            var time = date.AddHours(hours * -1);
            
            var customers = await _customerRepository.ListAllAsync();
            
            return await customers.Where(c => c.Country == country && c.Dispositions.Any(d => d.Account.Transactions
                    .Where(t => t.Date <= date && t.Date >= time)
                    .Sum(t => -t.Amount) > amount))
                .Select(c => new CustomerReport
                {
                    Id = c.CustomerId,
                    Name = $"{c.Givenname} {c.Surname}",
                    Accounts = c.Dispositions
                        .Where(a => a.Account.Transactions
                            .Where(t => t.Date <= date && t.Date >= time)
                            .Sum(t => -t.Amount) > amount)
                        .Select(d => new AccountReport
                        {
                            Id = d.Account.AccountId,
                            Transactions = d.Account.Transactions
                                .Where(t => t.Date <= date && t.Date >= time)
                                .Select(t => new TransactionReport()
                                {
                                    Id = t.TransactionId,
                                    Date = t.Date,
                                    Amount = t.Amount
                                })
                        })
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}