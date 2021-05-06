using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.ViewModels;
using Bank.Core.ViewModels.Statistics;
using Bank.Data.Data;
using Bank.Data.Models;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Customer;
using Bank.Data.Repositories.Disposition;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDispositionRepository _dispositionRepository;
        private readonly IMapper _mapper;

        public StatisticsService(ApplicationDbContext dbContext, IAccountRepository accountRepository, ICustomerRepository customerRepository, IDispositionRepository dispositionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _dispositionRepository = dispositionRepository;
            _mapper = mapper;
        }

        public IndexViewModel GetBaseStatistics()
        {
            return new()
            {
                TotalAccounts = _dbContext.Accounts.Count(),
                TotalCustomers = _dbContext.Customers.Count(),
                TotalBalance = _dbContext.Accounts.Sum(b => b.Balance),
            };
        }

        //TODO FIX ME
        public async Task<CountryStatisticsViewModel> GetCountryStatisticsAsync()
        {
            var model = new CountryStatisticsViewModel {Countries = new List<Item>()};

            var customers = (from customer in _dbContext.Customers
                let accounts = _dbContext.Dispositions.Include(a => a.Account)
                    .Where(i => i.CustomerId == customer.CustomerId/* && i.Type == "OWNER"*/)
                    .Select(i => i.Account)
                    .ToList()
                where accounts.Count != 0
                select new {Customer = customer, Accounts = accounts,}).ToList();

            var groupedCountries = customers.GroupBy(c => c.Customer.Country);

            foreach (var grouping in groupedCountries)
            {
                int customersAmount = 0;
                int accountsAmount = 0;
                decimal accountsTotalBalance = 0;

                foreach (var test in grouping)
                {
                    customersAmount += 1;
                    accountsAmount += test.Accounts.Count;
                    accountsTotalBalance += test.Accounts.Select(i => i.Balance).Sum();
                }

                model.Countries.Add(new Item
                {
                    Country = grouping.Key, 
                    CustomerAmount = customersAmount,
                    AccountAmount = accountsAmount,
                    AccountsTotalBalance = accountsTotalBalance
                });
            }

            return model;
        }

        //todo double check me.
        public async Task<IEnumerable<TopCustomerViewModel>> GetTopCustomersByCountry(int amount, string country)
        {
            var query = _dbContext.Dispositions
                .Include(a => a.Account)
                .Include(i => i.Customer)
                .Where(i => i.Customer.Country == country && i.Type == "OWNER")
                .OrderByDescending(i => i.Account.Balance)
                .Take(amount)
                .AsQueryable();

            var model = _mapper.Map<IEnumerable<TopCustomerViewModel>>(query);
            return model;
        }
    }
}