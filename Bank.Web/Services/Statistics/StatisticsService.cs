using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Data.Data;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Customer;
using Bank.Data.Repositories.Disposition;
using Bank.Web.Services.Customers;
using Bank.Web.ViewModels;
using Bank.Web.ViewModels.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Bank.Web.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public StatisticsService(ApplicationDbContext dbContext, ICustomerRepository customerRepository,  IMapper mapper)
        {
            _dbContext = dbContext;
            _customerRepository = customerRepository;
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

        public async Task<CountryStatisticsViewModel> GetCountryStatisticsAsync()
        {
            //
            var customersQuery = await _customerRepository.ListAllAsync();
            return new CountryStatisticsViewModel
            {
                Countries = (from customer in customersQuery
                        let accounts = _dbContext.Dispositions.Include(a => a.Account)
                            .Where(i => i.CustomerId == customer.CustomerId && i.Type == "OWNER")
                            .Select(i => i.Account)
                            .ToList()
                        where accounts.Count != 0
                        select new {Customer = customer, Accounts = accounts, TotalSum = accounts.Sum( i => i.Balance)})
                    .AsEnumerable().GroupBy(c => new {c.Customer.Country})
                    .Select(t => new Item()
                    {
                        Country = t.Key.Country,
                        CustomerAmount = t.Count(),
                        AccountAmount = t.Sum(c => c.Accounts.Count),
                        AccountsTotalBalance = t.Sum(i => i.TotalSum)
                    }).ToList()
            };
        }

        public async Task<IEnumerable<TopCustomerViewModel>> GetTopCustomersByCountry(int amount, string country)
        {
            var customersQuery = await  _customerRepository.ListAllAsync();
            var result = customersQuery
                .Include(d => d.Dispositions)
                .ThenInclude(a => a.Account)
                .Where(customer => customer.Country == country)
                .OrderByDescending(c => c.Dispositions.Sum(d => d.Account.Balance)) 
                .Take(amount);

            return _mapper.Map<IEnumerable<TopCustomerViewModel>>(result);
        }
    }
}