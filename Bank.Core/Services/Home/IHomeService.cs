using System.Linq;
using Bank.Core.ViewModels;
using Bank.Web.Data;

namespace Bank.Core.Services.Home
{
    public interface IHomeService
    {
        public IndexViewModel GetStats();
    }

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
