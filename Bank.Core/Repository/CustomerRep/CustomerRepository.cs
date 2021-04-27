using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Data;
using Bank.Core.Model;
using Bank.Core.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Repository.CustomerRep
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IQueryable<Customer>> GetPagedResponseAsync(int page, int size, string q)
        {
            return Task.FromResult(_dbContext.Set<Customer>()
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
