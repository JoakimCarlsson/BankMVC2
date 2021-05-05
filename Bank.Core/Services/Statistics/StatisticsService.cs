using System.Linq;
using Bank.Core.ViewModels;
using Bank.Data.Data;

namespace Bank.Core.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _dbContext;

        public StatisticsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IndexViewModel GetStatistics()
        {
            return new()
            {
                TotalAccounts = _dbContext.Accounts.Count(),
                TotalCustomers = _dbContext.Customers.Count(),
                TotalBalance = _dbContext.Accounts.Sum(b => b.Balance),
            };
        }
    }
}