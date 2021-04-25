using System.Linq;
using Bank.Core.Data;
using Bank.Core.ViewModels;

namespace Bank.Core.Services.Home
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IndexViewModel GetStats()
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