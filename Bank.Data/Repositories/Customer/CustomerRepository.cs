using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Data;
using Bank.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data.Repositories.Customer
{
    public class CustomerRepository : BaseRepository<Models.Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IQueryable<Models.Customer>> GetPagedResponseAsync(int page, int size, string q)
        {
            return Task.FromResult(_dbContext.Set<Models.Customer>()
                .Where(i => q == null || i.Givenname.ToLower().StartsWith(q.ToLower()) || i.City.ToLower().StartsWith(q.ToLower()))
                .Skip((page - 1) * size)
                .Take(size)
                .AsNoTracking().AsQueryable());
        }

        public Task<int> GetQueryCount(string q)
        {
            var result = _dbContext.Customers.AsQueryable();
            return result.Where(i => q == null || i.Givenname.ToLower().StartsWith(q.ToLower()) || i.City.ToLower().StartsWith(q.ToLower())).CountAsync();
        }
    }
}
